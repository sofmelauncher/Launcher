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
      
        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();


        private Subject<int> buttonNotification=new Subject<int>();
        public IObservable<int> ButtonNotification => buttonNotification;

        //Bind Properties
        public string GameName { get=>MyGameInfo.GameName;}
        public string IconPath {get => MyGameInfo.IconPath;}
        public string GameID{get; set;}
        public ReactiveProperty<double> MyScale { get; set;}

        public GamePanelViewModel(GameInfo game_info,int index_number) {
            MyGameInfo=game_info;
            GameID = (index_number.ToString()).PadLeft(2,'0');
        }

        public void SetPanelSizes(float scale) {
            PanelSizes=new PanelSizes(this.Disposable,scale);
            MyScale = PanelSizes.MyScale;
        }

        public void MouseClickSubmit() {
            try{
                GameProcessControll.GetInstance.GameLaunch(MyGameInfo.BinPath);
            } catch (Exception e){
                //plz use dialog to messeage that didnt run game 
            }
        }


        public void Dispose() {
            Disposable.Dispose();
        }
    }
}
