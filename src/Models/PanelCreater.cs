using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using meGaton.DataResources;
using meGaton.src.DataResources;
using meGaton.ViewModels;
using meGaton.Views;
using MaterialDesignThemes.Wpf;
using Moq;

namespace meGaton.Models{
    /// <summary>
    /// パネルファクトリ、データベースに接続しGameInfoの数だけパネルを作成します
    /// </summary>
    class PanelCreater {

        private readonly IGamesDataConnector iGamesDataConnector;//データベース

        public PanelCreater() {
            //テスト用モックデータベースの作成
           /* var moq = new Mock<IGamesDataConnector>();
            moq.Setup(m => m.GetGamesInfo())
                .Returns(new List<GameInfo>(){
                   });
            iGamesDataConnector =moq.Object;*/
            iGamesDataConnector=new GameInfoJsonReader();
        }

        //パネルの作成。ViewModelもここで抽入する
        public void Launch(Panel parent_panel){
            var counter = 1;
            var games_info = iGamesDataConnector.GetGamesInfo();
            if (games_info.Count == 0){
                Logger.Inst.Log("GamesList is Empty.I don't create panels.",LogLevel.Warning);
                throw new DataException();
            }

            foreach (var item in games_info){
                parent_panel.Children.Add(new GamePanel(new GamePanelViewModel(item,counter)));
                counter++;
            }
        }
    }
}
