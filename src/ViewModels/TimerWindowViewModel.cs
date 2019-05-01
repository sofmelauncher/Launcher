namespace meGaton.ViewModels {
    class TimerWindowViewModel {

        //バインド用プロパティ
        public string Messeage { get; private set; }

        /// <param name="now_minute">経過した分</param>
        public TimerWindowViewModel(int now_minute) {
            Messeage= "遊び始めてから" + now_minute + "分が経過しました";
        }
    }
}
