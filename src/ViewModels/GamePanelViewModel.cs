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
        private GameInfo myGameInfo;
        private PanelSizes panelSizes;
        private GameProcessControll gameProcessControll;


        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        private Subject<int> buttonNotification=new Subject<int>();
        public IObservable<int> ButtonNotification => buttonNotification;

        //Bind Properties
        public string GameName { get=>myGameInfo.GameName;}
        public string IconPath {get => myGameInfo.IconPath;}
        public ReactiveProperty<double> MyScale { get; set;}

        
        private void OkCommandExecute(object parameter) {
            gameProcessControll.GameLaunch(myGameInfo.BinPath);
        }

        private ICommand okCommand;
        public ICommand OkCommand {
            get {
                if (okCommand == null)
                    okCommand = new DelegateCommand {
                        ExecuteHandler = OkCommandExecute,
                    };
                return okCommand;
            }
        }


        public GamePanelViewModel(GameInfo game_info,GameProcessControll game_process_controll) {
            myGameInfo=game_info;
            gameProcessControll = game_process_controll;
        }

        public void SetPanelSizes(float scale) {
            panelSizes=new PanelSizes(this.Disposable,scale);
            MyScale = panelSizes.MyScale;
        }
        

        public void Dispose() {
            Disposable.Dispose();
        }


        public void Enlarge() {
            panelSizes.Enlarge();
        }
        public void Undo() {
            panelSizes.Undo();
        }

    }
}
