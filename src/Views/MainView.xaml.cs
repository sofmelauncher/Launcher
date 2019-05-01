using meGaton.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace meGaton.Views{
    /// <summary>
    /// MainView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window {

        public MainView() {
            InitializeComponent();

            //MainViewModelを経由して各Modelに各部品への参照を持たせる
            var view_model = new MainViewModel(
                this,
                FindName("PanelParent") as Panel
                , FindName("DisplayVideo") as MediaElement
                , FindName("ControlIconParent") as Panel
                , this.FindName("RootGrid") as Grid);
            this.DataContext = view_model;

        }

        //マウスホイールイベントがView層でしか拾えないためここにイベントがあるが処理はViewModelに委譲する
        protected override void OnMouseWheel(MouseWheelEventArgs e) {
            (this.DataContext as MainViewModel)?.MouseWheel(e.Delta);
        }
    }
}
