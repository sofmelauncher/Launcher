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
        private readonly GameProcessControll gameProcessControll=new GameProcessControll();

        public MainViewModel(PanelControler panel_controler,MediaDisplay media_display, ControllerDisplay controller_display) {
            GameDiscription = new ReactiveProperty<string>().AddTo(this.Disposable);
            mediaDisplay = media_display;

            controllerDisplay = controller_display;

            ListUpCommand.Subscribe(n => panel_controler.SlideDown());
            ListDownCommand.Subscribe(n => panel_controler.SlideUp());
            GamePadObserver.GetInstance.VerticalStickStream
                .Where(n => n != 0)
                .Subscribe(n => {
                    if (n == 1) {
                        panel_controler.SlideDown();
                    }else if (n == -1) {
                        panel_controler.SlideUp();
                    }
                });
            GamePadObserver.GetInstance.EnterKeyStream
                .Where(n=>n)
                .Subscribe(n =>gameProcessControll.GameLaunch(panel_controler.GetCurrentPanelsInfo.BinPath));


            panel_controler.ChangeSelectedPanel.Subscribe(ChangeSeletedDisplay);
            ChangeSeletedDisplay(panel_controler.GetCurrentPanelsInfo);
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
