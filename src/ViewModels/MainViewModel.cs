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
    /// </summary>
    public partial class MainViewModel : INotifyPropertyChanged, IDisposable {
        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public ReactiveProperty<string> GameDiscription { get; set; }
        public ReactiveCommand ListUpCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ListDownCommand { get; } = new ReactiveCommand();
        public ReactiveCommand TimerResetCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<Brush>[] ControllerIconColors => controllerDisplay.ColorList.ToArray();

        private readonly PanelController panelController;
        private readonly GameProcessControll gameProcessControll;
        private readonly MediaDisplay mediaDisplay;
        private readonly ControllerDisplay controllerDisplay;

        public MainViewModel(Window main_window,StackPanel panel_parent,MediaElement media_display,StackPanel controller_icon_parent) {
            GameDiscription = new ReactiveProperty<string>().AddTo(this.Disposable);

            mediaDisplay = new MediaDisplay(media_display);
            controllerDisplay = new ControllerDisplay(controller_icon_parent);
            gameProcessControll = GameProcessControll.GetInstance;

            try {
                new PanelCreater().Launch(panel_parent);

            } catch (Exception e){
                Logger.Inst.Log("I wanna stop my process bc GamePanels was didn't create.",LogLevel.Warning);
                return;
            }

            panelController = new PanelController(panel_parent);
            var customer_timer = new CustomerTimer(main_window);

            ListUpCommand.Subscribe(n => panelController.SlideDown());
            ListDownCommand.Subscribe(n => panelController.SlideUp());

            TimerResetCommand.Subscribe(n => {
                customer_timer.Stop();
                panelController.Shuffle();
            });

            GamePadObserver.GetInstance.VerticalStickStream
                .Where(n=>!gameProcessControll.IsRunning)
                .Where(n => n != 0)
                .Subscribe(n => {
                    if (n == 1) {
                        panelController.SlideDown();
                    }else if (n == -1) {
                        panelController.SlideUp();
                    }
                    customer_timer.StartRequest();
                });
            GamePadObserver.GetInstance.EnterKeyStream
                .Where(n => !gameProcessControll.IsRunning)
                .Where(n=>n)
                .Subscribe(n => {
                    gameProcessControll.GameLaunch(panelController.GetCurrentPanelsInfo.BinPath);
                    customer_timer.StartRequest();
                });


            panelController.ChangeSelectedPanel.Subscribe(ChangeSeletedDisplay);
            ChangeSeletedDisplay(panelController.GetCurrentPanelsInfo);
        }


        public void MouseWheel(int delta) {
            if (panelController == null){
                Logger.Inst.Log("My process don't running bc I didn't create Panel",LogLevel.Warning);
                return;
            }
            if (gameProcessControll.IsRunning) return;
            if (delta > 0) {
                panelController.SlideDown();
            } else if (delta < 0) {
                panelController.SlideUp();
            }
        }

        public void ChangeSeletedDisplay(GameInfo game_info) {
            try{
                GameDiscription.Value = game_info.GameDescription;
                mediaDisplay.SetMedia(game_info.PanelsPath, game_info.VideoPath);
                controllerDisplay.ChangeIcon(game_info.UseControllers);

            } catch (NullReferenceException e){
                Logger.Inst.Log(e+"My process don't running bc I didn't create Panel",LogLevel.Warning);
            }
        }

        public void Dispose() {
            Disposable.Dispose();
        }
    }
}
