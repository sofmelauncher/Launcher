using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Input;
using Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using meGaton.DataResources;
using meGaton.Models;
using Reactive.Bindings;

namespace meGaton.ViewModels{
    public partial class GamePanelViewModel : INotifyPropertyChanged,IDisposable {
        public GameInfo MyGameInfo { get; private set; }
        public PanelSizes PanelSizes { get; private set; }
        public ReactiveCommand ClickCommand { get; } = new ReactiveCommand();

        private GameProcessControll gameProcessControll;


        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();


        private Subject<int> buttonNotification=new Subject<int>();
        public IObservable<int> ButtonNotification => buttonNotification;

        //Bind Properties
        public string GameName { get=>MyGameInfo.GameName;}
        public string IconPath {get => MyGameInfo.IconPath;}
        public ReactiveProperty<double> MyScale { get; set;}

        public GamePanelViewModel(GameInfo game_info) {
            MyGameInfo=game_info;
            gameProcessControll = GameProcessControll.GetInstance;

            ClickCommand.Subscribe(n=>Console.WriteLine(MyGameInfo.GameName));
        }

        public void SetPanelSizes(float scale) {
            PanelSizes=new PanelSizes(this.Disposable,scale);
            MyScale = PanelSizes.MyScale;
        }

        public void MouseClickSubmit() {
            gameProcessControll.GameLaunch(MyGameInfo.BinPath);
        }


        public void Dispose() {
            Disposable.Dispose();
        }
    }
}
