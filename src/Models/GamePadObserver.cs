using System;
using System.Reactive.Subjects;
using System.Windows.Threading;
using Tao.Platform.Windows;

/// <summary>
/// it observe Gamepad to publish key event.
/// </summary>
/// <remarks>
/// reference <see cref="http://d.sunnyone.org/2012/09/taocwpf.html">
/// </remarks>>
namespace meGaton.Models
{
    class GamePadObserver {
        public static GamePadObserver GetInstance { get; } = new GamePadObserver();

        private Subject<bool> enterKeyStream=new Subject<bool>();
        public IObservable<bool> EnterKeyStream => enterKeyStream;
        private Subject<int> verticalStickStream=new Subject<int>();
        public IObservable<int> VerticalStickStream => verticalStickStream;

        private const int CENTER_POINT = 32767;
        private const int DEAD = 16000;

        public GamePadObserver() {
            var joyinfo = new Tao.Platform.Windows.Winmm.JOYINFO();

            var timer = new DispatcherTimer(DispatcherPriority.Normal) {Interval = new TimeSpan(0, 0, 0, 0, 10)};

            timer.Tick += new EventHandler((sender1, e1) => {
                if (Tao.Platform.Windows.Winmm.joyGetPos(0, ref joyinfo) == 0) {
                    enterKeyStream.OnNext((joyinfo.wButtons & Winmm.JOY_BUTTON1) != 0);
                    var ypos = joyinfo.wYpos;
                    var y_flow=0;
                    if (ypos < CENTER_POINT - DEAD) {
                        y_flow = 1;
                    }else if (ypos > CENTER_POINT + DEAD) {
                        y_flow = -1;
                    }
                    verticalStickStream.OnNext(y_flow);
                }
            });

            timer.Start();
        }
    }
}
