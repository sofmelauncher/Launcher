using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace meGaton.Util {
    public class HardWareObserver {
        private float cpuStack;
        public float CpuUsageAverage => measureCounter==0?0:(cpuStack / measureCounter);
        private float memoryStack;
        public float MemoryUsageAverage => measureCounter == 0 ? 0 : (memoryStack / measureCounter);
        private int measureCounter=-1;

        private PerformanceCounter cpuObserver;
        private PerformanceCounter memoryObserver;
        private DispatcherTimer dispatcherTimer;

        public async void Init() {
            if(measureCounter!=-1)return;
            measureCounter = 0;

            await Task.Run(() => {
                cpuObserver = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                memoryObserver = new PerformanceCounter("Memory", "Available MBytes");
            });

            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal) {
                Interval = new TimeSpan(0, 0, 0, 1)
            };
            dispatcherTimer.Tick += (sender, e) => {
                var cpu = cpuObserver.NextValue();
                var mem = memoryObserver.NextValue();
                if ((int) cpu== 0)return;//たまに使用率0になるのでその時は計測しない
                cpuStack += cpu;
                memoryStack += mem;
                measureCounter++;
            };
        }

        public void ObserveStart() {
            if (dispatcherTimer == null) {
                return;
            }
            dispatcherTimer.Start();
            cpuStack = 0;
            memoryStack = 0;
            measureCounter = 0;
        }

        public void ObserveEnd() {
            dispatcherTimer?.Stop();
        }
    }
}
