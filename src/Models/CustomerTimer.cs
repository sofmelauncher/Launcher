using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using meGaton.ViewModels;
using meGaton.Views;
using Tao.Platform.Windows;

namespace meGaton.Models {
    class CustomerTimer {
        private DateTime startTime;
        private TimeSpan nowtimespan;

        private int counter=1;

        private const int TIME_LIMIT_SECOND = 300;
        private const int NOTIFICATION_WINDOW_DISPLAY_MARGIN = 50;

        private Window mainWindow;
        private DispatcherTimer dispatcherTimer;

        public CustomerTimer(Window main_window) {
            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += DispatcherTimerTick;
            
            Start();

            mainWindow = main_window;
        }

        public void Start() {
            counter = 1;
            startTime = DateTime.Now;
            nowtimespan=new TimeSpan();
            dispatcherTimer.Start();
        }

        public void Stop() {
            dispatcherTimer.Stop();
        }

        void DispatcherTimerTick(object sender, EventArgs e) {
            nowtimespan = DateTime.Now.Subtract(startTime);
            if (TimeSpan.Compare(nowtimespan, new TimeSpan(0, 0, TIME_LIMIT_SECOND)) >= 0) {
                CreateWindow();
                nowtimespan=new TimeSpan();
                startTime = DateTime.Now;
            }
        }

        private void CreateWindow() {
            var temp = new TimerWindow {DataContext = new TimerWindowViewModel((TIME_LIMIT_SECOND / 60) * counter)};
            var desktop = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            temp.Top = desktop.Height - (temp.Height + NOTIFICATION_WINDOW_DISPLAY_MARGIN);
            temp.Left = desktop.Width - (temp.Width + NOTIFICATION_WINDOW_DISPLAY_MARGIN);
            temp.Owner = mainWindow;
            temp.Show();

            counter++;
        }
    }
}
