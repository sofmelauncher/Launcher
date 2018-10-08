using System.Windows;
using meGaton.Models;

namespace meGaton.Views {
    /// <summary>
    /// TimerWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TimerWindow : Window {
        public TimerWindow() {
            InitializeComponent();

            new InstantTimer(10, Close);
        }
    }
}
