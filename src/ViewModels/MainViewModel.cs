using Reactive.Bindings;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Controls;
using System.Windows.Input;
using meGaton.DataResources;
using meGaton.Models;
using Reactive.Bindings.Extensions;

namespace meGaton.ViewModels {
    /// <summary>
    /// MainViewModel.xaml の相互作用ロジック
    /// </summary>
    public partial class MainViewModel : INotifyPropertyChanged, IDisposable {

        public ReactiveProperty<string> GameDiscription { get; set; }
        public ReactiveCommand ListUpCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ListDownCommand { get; } = new ReactiveCommand();
        private readonly PanelControler panelControler;
        private readonly DisplayControll displayControll;


        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public MainViewModel(StackPanel stack_panel, MediaElement media_element, Image image) {
            GameDiscription = new ReactiveProperty<string>().AddTo(this.Disposable);
            panelControler = new PanelControler(stack_panel);
            displayControll=new DisplayControll(media_element,image);

            ListUpCommand.Subscribe(n => panelControler.SlideUp());
            ListDownCommand.Subscribe(n => panelControler.SlideDown());

            panelControler.ChangeSelectedPanel.Subscribe(ChangeSeleted);
            ChangeSeleted(panelControler.GetCurrentPanelsInfo);
        }

        private void ChangeSeleted(GameInfo game_info) {
            GameDiscription.Value = game_info.GameDescription;
            displayControll.SetMedia(game_info.PanelsPath,game_info.VideoPath);
        }

        public void Dispose() {
            Disposable.Dispose();
        }
    }
}
