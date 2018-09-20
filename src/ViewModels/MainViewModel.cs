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
        public ReactiveCommand MyCommand { get; } = new ReactiveCommand();
        private PanelControler panelControler;

        public MainViewModel(StackPanel stack_panel) {
            panelControler = new PanelControler(stack_panel);
            MyCommand.Subscribe(n => panelControler.SlidePanels());
        }
    }
}
