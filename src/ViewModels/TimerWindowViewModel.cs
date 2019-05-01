using Reactive.Bindings;

namespace meGaton.ViewModels {
    class TimerWindowViewModel {

        //バインド用プロパティ
        public string Messeage { get; private set; }

        //あくまで表示が責任のため時間の変換なども行わない
        public TimerWindowViewModel(int now_second) {
            Messeage= "遊び始めてから" + now_second + "分が経過しました";
        }
    }
}
