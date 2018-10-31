using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Input;
using Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using meGaton.DataResources;
using meGaton.Models;
using meGaton.src.Models;
using Reactive.Bindings;

namespace meGaton.ViewModels{
    /// <summary>
    /// GamePanelの各種プロパティや拡大縮小の操作、及びGameInfoの保持を行う。
    /// </summary>
    public partial class GamePanelViewModel : INotifyPropertyChanged,IDisposable {
        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();


        public GameInfo MyGameInfo { get; private set; }
        public PanelSizes PanelSizes { get; private set; }

        
        //バインド用プロパティ
        public string GameName { get=>MyGameInfo.GameName.ReplaceNewLineCodeAndIndent();}
        public string IconPath {get => PathManage.GAMES_ROOT_PATH+"\\" + MyGameInfo.IconPath;}
        public string GameID{get; set;}
        public ReactiveProperty<double> MyScale => PanelSizes.MyScale;


        //index_numberは展示用IDと同義
        public GamePanelViewModel(GameInfo game_info,int index_number) {
            MyGameInfo=game_info;
            GameID = (index_number.ToString()).PadLeft(2,'0');
        }

        //見た目のプロパティのためここはViewにセットしてもらう必要がある
        public void SetPanelLargeScale(float scale) {
            PanelSizes=new PanelSizes(this.Disposable,scale);
        }

        //Viewで受け取ったパネルクリックイベントの処理
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
