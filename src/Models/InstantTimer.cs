using System;
using System.Windows.Threading;

namespace meGaton.Models {
    public class InstantTimer {
        public InstantTimer(int limit,Action action) {
            var dispatcher_timer = new DispatcherTimer(DispatcherPriority.Normal) {Interval = new TimeSpan(0, 0, limit)};
            dispatcher_timer.Tick += (sender, e) => {
                action.Invoke();
                dispatcher_timer.Stop();
                dispatcher_timer = null;
            };
            dispatcher_timer.Start();
        }
    }
}