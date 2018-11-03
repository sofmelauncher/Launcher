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
using meGaton.Models;
using meGaton.src.Models;
using Reactive.Bindings.Extensions;

namespace meGaton.ViewModels {
    /// <summary>
    /// MainViewModel.xaml の相互作用ロジック
    /// UI要素を必要とするModel全ての生成責任を持つ
    ///　ウィンドウ1枚のアプリなのもあって肥大化しがち
    /// </summary>
    public partial class MainViewModel : INotifyPropertyChanged, IDisposable {
        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();


        //バインド用プロパティ
        public ReactiveProperty<string> GameDiscription { get; set; }
        public ReactiveCommand ListUpCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ListDownCommand { get; } = new ReactiveCommand();
        public ReactiveCommand TimerResetCommand { get; } = new ReactiveCommand();
        public ReactiveCommand EnterKeyCommand { get; }=new ReactiveCommand();
        public ReactiveProperty<Brush>[] ControllerIconColors => controllerDisplay.ColorList.ToArray();

        private Subject<Unit> gameLaunchStream=new Subject<Unit>();
        private Subject<int> panelSlideStream=new Subject<int>();
        private Subject<int> panelSkipStream=new Subject<int>();

        //Model
        private readonly MediaDisplay mediaDisplay;
        private readonly ControllerDisplay controllerDisplay;

        public MainViewModel(Window main_window,Panel panel_parent,MediaElement media_display,Panel controller_icon_parent,Panel root_grid) {

            GameDiscription = new ReactiveProperty<string>().AddTo(this.Disposable);

            mediaDisplay = new MediaDisplay(media_display);
            controllerDisplay = new ControllerDisplay(controller_icon_parent);

            //ここでパネル生成できなかった場合各種プロセスは動作させない
            try {
                new PanelCreater().Launch(panel_parent);
            } catch (Exception e){
                Logger.Inst.Log("I wanna stop my process bc GamePanels was didn't create.",LogLevel.Warning);
                return;
            }

            var panel_controller = new PanelController(panel_parent);
            var customer_timer = new CustomerTimer(main_window);
            var mask_controll = new MaskControll(root_grid);

            //キー入力はViewModelでバインドされている
            EnterKeyCommand.Subscribe(n => gameLaunchStream.OnNext(Unit.Default));
            ListUpCommand.Subscribe(n => panelSlideStream.OnNext(1));
            ListDownCommand.Subscribe(n => panelSlideStream.OnNext(-1));

            TimerResetCommand.Subscribe(n => {
                customer_timer.Stop();
                panel_controller.Shuffle();
            });


            //リスト移動入力の定義
            panelSlideStream
                .Merge(GamePadObserver.GetInstance.VerticalStickEvent.Sample(TimeSpan.FromMilliseconds(200)))
                .Where(n=>!GameProcessControll.GetInstance.IsRunning)
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
                .Merge(GamePadObserver.GetInstance.HorizontalStickEvent.Sample(TimeSpan.FromMilliseconds(150)))
                .Where(n => !GameProcessControll.GetInstance.IsRunning)
                .Where(n => n != 0)
                .Subscribe(n => { panel_controller.Skip(n); });

            //ゲーム起動入力の定義
            gameLaunchStream
                .Merge(GamePadObserver.GetInstance.EnterKeyStream.Where(n => n).Select(n=>Unit.Default))
                .Merge(panel_controller.PanelClickEvent)
                .Subscribe(n => {
                    GameProcessControll.GetInstance.GameLaunch(panel_controller.GetCurrentPanelsInfo.MyGameInfo.BinPath);
                });

            //ゲーム起動時
            GameProcessControll.GetInstance.OnGameStart.Subscribe(n => {
                customer_timer.StartRequest();
                mask_controll.Run();
                mediaDisplay.Pause();
            });

            //ゲーム終了時
            GameProcessControll.GetInstance.OnGameEnd.Subscribe(n => {
                mask_controll.Remove();
                mediaDisplay.ReStart();
            });

            //一応起動時もシャッフル
            panel_controller.Shuffle();

            //PanelControllerの選択切り替えイベントを受け取る
            //最初の選択処理だけは実行する必要がある
            panel_controller.ChangeSelectedPanel.Subscribe(ChangeSeletedDisplay);
            ChangeSeletedDisplay(panel_controller.GetCurrentPanelsInfo);
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
        public void ChangeSeletedDisplay(GamePanelViewModel game_panel_view_model){
            var game_info = game_panel_view_model.MyGameInfo;
            if (game_info == null){
                Logger.Inst.Log(new ArgumentException()+"Can't select bc GameInfo is empty.");
                return;
            }

            try {
                GameDiscription.Value = game_info.GameDescription.ReplaceNewLineCode();
                mediaDisplay.SetMedia(game_info.PanelsPath, game_info.VideoPath);
                controllerDisplay.ChangeIcon(game_info.UseControllers);
            } catch (NullReferenceException e){
                Logger.Inst.Log(e+"GameInfo include null property.");
            }
        }

        public void Dispose() {
            Disposable.Dispose();
        }
    }
}
