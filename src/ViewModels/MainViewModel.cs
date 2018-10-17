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
        public ReactiveProperty<Brush>[] ControllerIconColors => controllerDisplay.ColorList.ToArray();

        private CustomerTimer customerTimer;
        private readonly MediaDisplay mediaDisplay;
        private readonly ControllerDisplay controllerDisplay;

        public MainViewModel(Window main_window,StackPanel panel_parent,MediaElement media_display,StackPanel controller_icon_parent) {
            GameDiscription = new ReactiveProperty<string>().AddTo(this.Disposable);

            customerTimer=new CustomerTimer(main_window);
            var panel_creater = new PanelCreater();
            panel_creater.Launch(panel_parent);
            var panel_controller = new PanelController(panel_parent);
            mediaDisplay = new MediaDisplay(media_display);
            controllerDisplay = new ControllerDisplay(controller_icon_parent);
            var game_process_controll = new GameProcessControll();

            ListUpCommand.Subscribe(n => panel_controller.SlideDown());
            ListDownCommand.Subscribe(n => panel_controller.SlideUp());
            GamePadObserver.GetInstance.VerticalStickStream
                .Where(n=>!game_process_controll.IsRunning)
                .Where(n => n != 0)
                .Subscribe(n => {
                    if (n == 1) {
                        panel_controller.SlideDown();
                    }else if (n == -1) {
                        panel_controller.SlideUp();
                    }
                });
            GamePadObserver.GetInstance.EnterKeyStream
                .Where(n => !game_process_controll.IsRunning)
                .Where(n=>n)
                .Subscribe(n =>game_process_controll.GameLaunch(panel_controller.GetCurrentPanelsInfo.BinPath));


            panel_controller.ChangeSelectedPanel.Subscribe(ChangeSeletedDisplay);
            ChangeSeletedDisplay(panel_controller.GetCurrentPanelsInfo);
        }

        public void ChangeSeletedDisplay(GameInfo game_info) {
            GameDiscription.Value = game_info.GameDescription;
            mediaDisplay.SetMedia(game_info.PanelsPath,game_info.VideoPath);
            controllerDisplay.ChangeIcon(game_info.UseControllers);
        }

        public void Dispose() {
            Disposable.Dispose();
        }
    }
}
