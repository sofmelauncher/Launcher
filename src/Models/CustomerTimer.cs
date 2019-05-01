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

        private bool isRunning;
        private int counter = 1;

        private DateTime startTime;
        private TimeSpan nowTimeSpan;
        private readonly DispatcherTimer dispatcherTimer;
        private Window currentTimer;

        
        private const int TIME_LIMIT_SECOND = 300;

        
        public CustomerTimer(){
            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal){
                Interval = new TimeSpan(0, 0, 1)
            };
            dispatcherTimer.Tick += (sender, e) =>{
                nowTimeSpan = DateTime.Now.Subtract(startTime);
                if (TimeSpan.Compare(nowTimeSpan, new TimeSpan(0, 0, TIME_LIMIT_SECOND)) >= 0){
                    CreateWindow();
                    nowTimeSpan = new TimeSpan();
                    startTime = DateTime.Now;
                }
            };
            StartRequest();
        }

        /// <summary>
        /// タイマー開始のリクエストを行います
        /// </summary>
        public void StartRequest(){
            if (isRunning) return;
            counter = 1;
            startTime = DateTime.Now;
            nowTimeSpan = new TimeSpan();
            dispatcherTimer.Start();
            isRunning = true;
        }

        /// <summary>
        /// タイマーを停止しウィンドウを削除します
        /// </summary>
        public void Stop(){
            dispatcherTimer.Stop();
            isRunning = false;
            currentTimer?.Close();
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
