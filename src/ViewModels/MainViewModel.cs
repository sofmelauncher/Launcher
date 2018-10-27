using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using meGaton.DataResources;
using meGaton.Models;
using MaterialDesignThemes.Wpf;
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
        public ReactiveProperty<Brush>[] ControllerIconColors => controllerDisplay.ColorList.ToArray();


        //Model
        private readonly PanelController panelController;
        private readonly MediaDisplay mediaDisplay;
        private readonly ControllerDisplay controllerDisplay;

        public MainViewModel(Window main_window,Panel panel_parent,MediaElement media_display,Panel controller_icon_parent) {

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

            panelController = new PanelController(panel_parent);
            var customer_timer = new CustomerTimer(main_window);


            //キー入力はViewModelでバインドされている
            ListUpCommand.Subscribe(n => panelController.SlideDown());
            ListDownCommand.Subscribe(n => panelController.SlideUp());

            TimerResetCommand.Subscribe(n => {
                customer_timer.Stop();
                panelController.Shuffle();
            });

            //GamePadObserberの垂直入力をPanelControllerに流す
            GamePadObserver.GetInstance.VerticalStickStream
                .Where(n=>!GameProcessControll.GetInstance.IsRunning)
                .Where(n => n != 0)
                .Subscribe(n => {
                    if (n == 1) {
                        panelController.SlideDown();
                    }else if (n == -1) {
                        panelController.SlideUp();
                    }
                });
            //GamePadObserberの決定入力をGameProcessControllに流す
            GamePadObserver.GetInstance.EnterKeyStream
                .Where(n=>n)
                .Subscribe(n => {
                    GameProcessControll.GetInstance.GameLaunch(panelController.GetCurrentPanelsInfo.MyGameInfo.BinPath);
                    customer_timer.StartRequest();
                });

            //PanelControllerの選択切り替えイベントを受け取る
            //最初の選択処理だけは実行する必要がある
            panelController.ChangeSelectedPanel.Subscribe(ChangeSeletedDisplay);
            ChangeSeletedDisplay(panelController.GetCurrentPanelsInfo);
        }

        //Viewから流れてくるマウスホイールイベントの処理
        public void MouseWheel(int delta) {
            if (panelController == null){
                return;//パネルなし時のプロセス停止時でもメソッドは止まらないのでPanelControllerの有無で実行可否を判定する
            }
            if (GameProcessControll.GetInstance.IsRunning) return;
            if (delta > 0) {
                panelController.SlideDown();
            } else if (delta < 0) {
                panelController.SlideUp();
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
                GameDiscription.Value = game_info.GameDescription;
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
