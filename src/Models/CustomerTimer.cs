using System;
using System.Windows;
using System.Windows.Threading;
using meGaton.ViewModels;
using meGaton.Views;

namespace meGaton.Models {
    /// <summary>
    /// プレイ時間を計測する。
    /// </summary>
    public class CustomerTimer:IDisposable {
        private DateTime startTime;
        private TimeSpan nowtimespan;

        private bool isRunning;
        private int counter = 1;


        private const int TIME_LIMIT_SECOND = 300;

        private Window mainWindow;
        private DispatcherTimer dispatcherTimer;
        private Window currentTimer;

        public CustomerTimer(Window main_window)
        {
            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += (sender, e) =>
            {
                nowtimespan = DateTime.Now.Subtract(startTime);
                if (TimeSpan.Compare(nowtimespan, new TimeSpan(0, 0, TIME_LIMIT_SECOND)) >= 0)
                {
                    CreateWindow();
                    nowtimespan = new TimeSpan();
                    startTime = DateTime.Now;
                }
            };

            mainWindow = main_window;
            StartRequest();
        }

        //リクエスト自体は何度も送られてくる想定
        public void StartRequest()
        {
            if (isRunning) return;
            counter = 1;
            startTime = DateTime.Now;
            nowtimespan = new TimeSpan();
            dispatcherTimer.Start();
            isRunning = true;
        }

        public void Stop()
        {
            dispatcherTimer.Stop();
            isRunning = false;
        }


        private void CreateWindow(){
            currentTimer = new TimerWindow {DataContext = new TimerWindowViewModel((TIME_LIMIT_SECOND / 60) * counter)};
            currentTimer.Show();
            currentTimer.Closed += (e, sender) => { currentTimer = null; };
            counter++;
        }

        public void Dispose() {
            currentTimer?.Close();
        }
    }
}
