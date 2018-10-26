using meGaton.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using meGaton.Models;

namespace meGaton.Views
{
    /// <summary>
    /// MainView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window {

        public MainView() {
            InitializeComponent();

            //MainnViewModelを経由して各Modelに各部品への参照を持たせる
            var view_model = new MainViewModel(
                this,
                FindName("PanelParent") as StackPanel
                , FindName("DisplayVideo") as MediaElement
                , FindName("ControllIconParent") as StackPanel);
            this.DataContext = view_model;
        }

        //マウスホイールイベントがView層でしか拾えないためここにイベントがあるが処理はViewModelに委譲する
        protected override void OnMouseWheel(MouseWheelEventArgs e) {
            (this.DataContext as MainViewModel).MouseWheel(e.Delta);
        }
    }
}
