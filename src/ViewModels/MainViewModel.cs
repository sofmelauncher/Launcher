using meGaton.src.Models;
using Reactive.Bindings;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace meGaton.ViewModels {
    /// <summary>
    /// MainViewModel.xaml の相互作用ロジック
    /// </summary>
    public partial class MainViewModel {
        public ReactiveCommand ListUpCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ListDownCommand { get; } = new ReactiveCommand();
        private readonly PanelControler panelControler;

        public MainViewModel(StackPanel stack_panel) {
            panelControler = new PanelControler(stack_panel);
            ListUpCommand.Subscribe(n => panelControler.SlideUp());
            ListDownCommand.Subscribe(n => panelControler.SlideDown());
        }
    }
}
