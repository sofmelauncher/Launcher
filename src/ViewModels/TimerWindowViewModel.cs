using Reactive.Bindings;

namespace meGaton.ViewModels {
    class TimerWindowViewModel {

        public string Messeage { get; private set; }

        public TimerWindowViewModel(int now_second) {
            Messeage= "遊び始めてから" + now_second + "分が経過しました";
        }
    }
}
