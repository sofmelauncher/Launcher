using System;
using System.Windows.Threading;

namespace meGaton.Models {
    /// <summary>
    /// n秒後に処理を実行する時に使う。
    /// このクラスが量産されるとDispatcherTimerも量産されるのでパフォーマンスが不味い気がする。
    /// InstantTimerが共通で１つのDispatcherTimer持つようにしたりとか要改善
    /// </summary>
    public class InstantTimer {
        private DispatcherTimer dispatcherTimer;
        //second秒後に引数なしの関数Actionを実行
        public InstantTimer(int second,Action action) {
            try{
                dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = new TimeSpan(0, 0, second) };
                dispatcherTimer.Tick += (sender, e) => {
                    action.Invoke();
                    dispatcherTimer.Stop();
                    dispatcherTimer = null;
                };
                dispatcherTimer.Start();
            }catch (Exception e){
                Logger.Inst.Log("InstantTimer Clashed");
                throw;
            }
        }

        public void Close() {
            dispatcherTimer.Stop();
        }
    }
}