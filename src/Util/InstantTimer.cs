using System;
using System.Windows.Threading;

namespace meGaton.Util {
    /// <summary>
    /// n秒後に処理を実行する時に使う。
    /// このクラスが量産されるとDispatcherTimerも量産されるのでパフォーマンスが不味い気がする。
    /// InstantTimerが共通で１つのDispatcherTimer持つようにしたりとか要改善
    /// スレッドの関係でサクっとこういう処理が行えないのでクラスにした
    /// 一回使い切りなので複数回実行する場合は各自DispatcherTimer使ってねって感じ
    /// </summary>
    public class InstantTimer {
        private DispatcherTimer dispatcherTimer;
        
        /// <param name="second">予約実行秒数（sec）</param>
        /// <param name="action">実行する処理</param>
        public InstantTimer(int second,Action action) {
            try{
                dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal){
                    Interval = new TimeSpan(0, 0, second)
                };
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

        /// <summary>
        /// 処理の予約実行を停止します
        /// </summary>
        public void Close() {
            dispatcherTimer.Stop();
        }
    }
}