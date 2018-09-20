﻿using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Input;
using meGaton.DataClass;
using Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace meGaton.ViewModels{
    public partial class GamePanelViewModel : INotifyPropertyChanged,IDisposable {
        private GameInfo myGameInfo;
        
        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        private Subject<int> buttonNotification=new Subject<int>();
        public IObservable<int> ButtonNotification => buttonNotification;

        //Bind Properties
        public string GameName { get; set; }
        public PanelSizes PanelSizes { get; set;}
        

        private bool a;

        private void OkCommandExecute(object parameter) {
            //buttonNotification.OnNext(0);
            a = !a;
            if (a) {
                PanelSizes.Enlarge();
            } else {
                PanelSizes.Undo();
            }
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


        public GamePanelViewModel(string name) {
            GameName = name;

        }

        public void SetPanelSizes(float panel_width, float panel_height, int font_size, float icon_size,float scale) {
            PanelSizes=new PanelSizes(this.Disposable,panel_width,panel_height,font_size,icon_size,scale);
        }
        

        public void Dispose() {
            Disposable.Dispose();
        }
    }
}
