using System.Windows;
using meGaton.Models;
using meGaton.Util;

namespace meGaton.Views {
    /// <summary>
    /// 立ち上がってからDELETE_TIME秒後に消滅する
    /// </summary>
    public partial class TimerWindow : Window
    {
        private const int DELETE_TIME=10;
        private const int NOTIFICATION_WINDOW_DISPLAY_MARGIN = 50;

        public TimerWindow() {
            InitializeComponent();
            var desktop = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            this.Top = desktop.Height - (this.Height + NOTIFICATION_WINDOW_DISPLAY_MARGIN);
            this.Left = desktop.Width - (this.Width + NOTIFICATION_WINDOW_DISPLAY_MARGIN);

            new InstantTimer(DELETE_TIME, Close);
        }
    }
}
