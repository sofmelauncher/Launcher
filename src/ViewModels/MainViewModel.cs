using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
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
        public ReactiveProperty<Brush>[] ControllerIconColors { get; private set; }


        private readonly MediaDisplay mediaDisplay;
        private readonly ControllerDisplay controllerDisplay;

        public MainViewModel(StackPanel games_parent, MediaElement media_element, StackPanel controller_parent) {
            GameDiscription = new ReactiveProperty<string>().AddTo(this.Disposable);
            mediaDisplay=new MediaDisplay(media_element);

     
            ControllerIconColors = Enumerable.Range(0,controller_parent.Children.OfType<UIElement>().Count(n=>n is PackIcon))
                .Select(_=>new ReactiveProperty<Brush>().AddTo(Disposable)).ToArray();
            controllerDisplay = new ControllerDisplay(controller_parent);

            var panel_controler = new PanelControler(games_parent);
            ListUpCommand.Subscribe(n => panel_controler.SlideDown());
            ListDownCommand.Subscribe(n => panel_controler.SlideUp());

            panel_controler.ChangeSelectedPanel.Subscribe(ChangeSeletedDisplay);
            ChangeSeletedDisplay(panel_controler.GetCurrentPanelsInfo);
        }

        public void ChangeSeletedDisplay(GameInfo game_info) {
            GameDiscription.Value = game_info.GameDescription;
            mediaDisplay.SetMedia(game_info.PanelsPath,game_info.VideoPath);
            controllerDisplay.ChangeIcon(game_info.UseControllers,ControllerIconColors.ToList());
        }

        public void Dispose() {
            Disposable.Dispose();
        }
    }
}
