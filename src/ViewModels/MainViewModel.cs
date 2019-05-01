using Reactive.Bindings;
using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using meGaton.DataResources;
using meGaton.Models;
using meGaton.Util;
using Reactive.Bindings.Extensions;

namespace meGaton.ViewModels {
    /// <summary>
    /// MainViewModel.xaml の相互作用ロジック
    /// UI要素を必要とするModel全ての生成責任を持つ
    /// 依存関係の抽入は全てここで行う
    /// </summary>
    public partial class MainViewModel : INotifyPropertyChanged, IDisposable {
        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();


        //バインド用プロパティ
        public ReactiveProperty<string> GameDescription { get; set; }
        public ReactiveCommand ListUpCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ListDownCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ListSkipUpCommand { get; }=new ReactiveCommand();
        public ReactiveCommand ListSkipDownCommand { get; } = new ReactiveCommand();
        public ReactiveCommand TimerResetCommand { get; } = new ReactiveCommand();
        public ReactiveCommand EnterKeyCommand { get; }=new ReactiveCommand();
        public ReactiveProperty<Brush>[] ControllerIconColors => controllerDisplay.ColorList.ToArray();

        
        private readonly Subject<Unit> gameLaunchStream=new Subject<Unit>();
        private readonly Subject<int> panelSlideStream=new Subject<int>();
        private readonly Subject<int> panelSkipStream=new Subject<int>();

        //Model
        private readonly MediaDisplay mediaDisplay;
        private readonly ControllerDisplay controllerDisplay;

        public MainViewModel(Window main_window,Panel panel_parent,MediaElement media_display,Panel controller_icon_parent,Panel root_grid) {

            GameDescription = new ReactiveProperty<string>().AddTo(this.Disposable);

            mediaDisplay = new MediaDisplay(media_display);
            controllerDisplay = new ControllerDisplay(controller_icon_parent);

            //ここでパネル生成できなかった場合各種プロセスは動作させない
            try {
                new PanelCreator(new GameInfoJsonReader()).Launch(panel_parent);
            } catch (Exception e){
                Logger.Inst.Log("I wanna stop my process bc GamePanels was didn't create.",LogLevel.Warning);
                return;
            }

            var panel_controller = new PanelController(panel_parent);
            var customer_timer = new CustomerTimer();
            var mask_control = new MaskControl(root_grid);

            main_window.Closed+= (e, sender) =>{
                customer_timer.Dispose();
            };
            //キー入力はViewにバインドされているので動作の定義だけする
            //エンター
            EnterKeyCommand.Subscribe(n => gameLaunchStream.OnNext(Unit.Default));
            //上下移動
            ListUpCommand.Subscribe(n => panelSlideStream.OnNext(1));
            ListDownCommand.Subscribe(n => panelSlideStream.OnNext(-1));
            //スキップ入力
            ListSkipUpCommand.Subscribe(n => panelSkipStream.OnNext(-1));
            ListSkipDownCommand.Subscribe(n => panelSkipStream.OnNext(1));
            //リセット
            TimerResetCommand.Subscribe(n => {
                customer_timer.Stop();
                panel_controller.Shuffle();
            });
            
            //リスト移動入力の定義
            panelSlideStream
                .Merge(GamePadObserver.Inst.InVerticalStickEvent.Sample(TimeSpan.FromMilliseconds(200)))
                .Where(n=>!GameProcessControl.Inst.IsRunning)
                .Where(n => n != 0)
                .Subscribe(n => {
                    if (n == 1) {
                        panel_controller.MoveUp();
                    }else if (n == -1) {
                        panel_controller.MoveDown();
                    }
                });

            //スキップ入力の定義
            panelSkipStream
                .Merge(GamePadObserver.Inst.InHorizontalStickEvent.Sample(TimeSpan.FromMilliseconds(150)))
                .Where(n => !GameProcessControl.Inst.IsRunning)
                .Where(n => n != 0)
                .Subscribe(n => { panel_controller.Skip(n); });

            //ゲーム起動入力の定義
            gameLaunchStream
                .Merge(GamePadObserver.Inst.OnEnterKeyDown.Where(n => n).Select(n=>Unit.Default))
                .Merge(panel_controller.OnPanelClick)
                .Subscribe(n => {
                    GameProcessControl.Inst.GameLaunch(panel_controller.GetCurrentPanelsInfo.MyGameInfo.BinPath);
                });

            //ゲーム起動時のイベント
            GameProcessControl.Inst.OnGameStart.Subscribe(n => {
                customer_timer.StartRequest();
                mask_control.Run();
                mediaDisplay.Pause();
            });

            //ゲーム終了時のイベント
            GameProcessControl.Inst.OnGameEnd.Subscribe(n => {
                mask_control.Remove();
                mediaDisplay.ReStart();
            });

            //一応起動時もシャッフル
            panel_controller.Shuffle();

            //PanelControllerの選択切り替えイベントを受け取る
            //最初の選択処理だけは実行する必要がある
            panel_controller.OnChangeSelected.Subscribe(ChangeSelectedDisplay);
            ChangeSelectedDisplay(panel_controller.GetCurrentPanelsInfo);
        }

        //Viewから流れてくるマウスホイールイベントの処理
        public void MouseWheel(int delta) {
            if (delta > 0) {
                panelSlideStream.OnNext(1);
            } else if (delta < 0) {
                panelSlideStream.OnNext(-1);
            }
        }

        //選択中のパネルからGameInfoを取り出して各UIに表示する
        private void ChangeSelectedDisplay(GamePanelViewModel game_panel_view_model){
            var game_info = game_panel_view_model.MyGameInfo;
            if (game_info == null){
                Logger.Inst.Log(new ArgumentException()+"Can't select bc GameInfo is empty.");
                return;
            }

            try {
                GameDescription.Value = game_info.GameDescription.ReplaceNewLineCode();
                mediaDisplay.SetMedia(game_info.VideoPath,game_info.PanelsPath);
                controllerDisplay.ChangeIconActive(game_info.UseControllers);
            } catch (NullReferenceException e){
                Logger.Inst.Log(e+"GameInfo include null property.");
            }
        }

        public void Dispose() {
            Disposable.Dispose();
        }
    }
}
