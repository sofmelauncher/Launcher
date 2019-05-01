using System;
using System.Reactive.Subjects;
using System.Windows.Threading;
using Tao.Platform.Windows;

namespace meGaton.Models{
    /// <summary>
    /// ゲームパッドを監視しイベントに変換するシングルトン
    /// </summary>
    /// <remarks>
    /// reference <see cref="http://d.sunnyone.org/2012/09/taocwpf.html">
    /// </remarks>
    public class GamePadObserver {
        
        public static GamePadObserver Inst { get; } = new GamePadObserver();

        private readonly Subject<bool> onEnterKeyDown=new Subject<bool>();
        /// <summary>エンターキーが押された</summary>
        public IObservable<bool> OnEnterKeyDown => onEnterKeyDown;
        private readonly Subject<int> inVerticalInput=new Subject<int>();
        /// <summary>垂直キーが倒されている</summary>
        public IObservable<int> InVerticalStickEvent => inVerticalInput;
        private readonly Subject<int> inHorizontalInput=new Subject<int>();
        /// <summary>水平キーが倒されている</summary>
        public IObservable<int> InHorizontalStickEvent => inHorizontalInput;

        
        private readonly int CENTER_POINT = 32767;//スライドパッドはuInt16で表現されているので中央値がこれになる。
        private readonly int DEAD = 16000;//スライドパッドが入力の閾値。16000だとだいたい中央から半分以上倒したら入力扱い


        //この辺の処理は殆ど参考URLのパクリ。ゲームパッドの接続数を１つに限定している
        private GamePadObserver() {
            var joy_info = new Winmm.JOYINFO();

            var timer = new DispatcherTimer(DispatcherPriority.Normal){
                Interval = new TimeSpan(0, 0, 0, 0, 10)
            };

            timer.Tick += (sender1, e1) => {
                if (Winmm.joyGetPos(0, ref joy_info) == 0) {
                    onEnterKeyDown.OnNext((joy_info.wButtons & Winmm.JOY_BUTTON1) != 0);
                    var x_pos = joy_info.wXpos;
                    var y_pos = joy_info.wYpos;
                    var y_flow=0;
                    var x_flow = 0;
                    if (y_pos < CENTER_POINT - DEAD) {
                        y_flow = 1;
                    }else if (y_pos > CENTER_POINT + DEAD) {
                        y_flow = -1;
                    }
                    if (x_pos > CENTER_POINT + DEAD) {
                        x_flow = 1;
                    } else if (x_pos < CENTER_POINT - DEAD) {
                        x_flow = -1;
                    }
                    inHorizontalInput.OnNext(x_flow);
                    inVerticalInput.OnNext(y_flow);
                }
            };

            timer.Start();
        }
    }
}
