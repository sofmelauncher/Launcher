using System;
using System.Windows.Threading;

namespace meGaton.Models {
    /// <summary>
    /// n秒後に処理を実行する時に使う。
    /// このクラスが量産されるとDispatcherTimerも量産されるのでパフォーマンスが不味い気がする。
    /// InstantTimerが共通で１つのDispatcherTimer持つようにしたりとか要改善
    /// </summary>
    public class InstantTimer {
        //second秒後に引数なしの関数Actionを実行
        public InstantTimer(int second,Action action) {
            try{
                var dispatcher_timer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = new TimeSpan(0, 0, second) };
                dispatcher_timer.Tick += (sender, e) => {
                    action.Invoke();
                    dispatcher_timer.Stop();
                    dispatcher_timer = null;
                };
                dispatcher_timer.Start();
            }catch (Exception e){
                Logger.Inst.Log("InstantTimer Clashed");
                throw;
            }
        }
    }
}