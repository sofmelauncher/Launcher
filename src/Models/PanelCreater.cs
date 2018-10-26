using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using meGaton.DataResources;
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
            var root = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')+ "\\Games";
            var moq = new Mock<IGamesDataConnector>();
            moq.Setup(m => m.GetGamesInfo())
                .Returns(new List<GameInfo>(){
                    new GameInfo("TestGame1","I am the bone of my sword. ",0,root+"\\1\\game\\DOUTONBORI.exe",root+"\\1\\panel\\icon.png",new string[]{root+"\\1\\panel\\img1.png"},"",new GameController[0], new Tag[0]),
                    new GameInfo("TestGame2","Steel is my body,and fire is my blood. ",0,root+"\\2\\game\\STG.exe",root+"\\2\\panel\\icon.png",new string[]{ root + "\\2\\panel\\img1.png" },"",new GameController[0], new Tag[0]),
                    new GameInfo("TestGame3","I have created over a thousand blades. ",0,root+"\\3\\game\\DOUTONBORI.exe",root+"\\3\\panel\\icon.png",new string[]{ root + "\\3\\panel\\img1.png" }
                        ,root+"\\1\\panel\\video.mp4",new GameController[]{GameController.Mouse,GameController.Xbox}, new Tag[]{new Tag("RPG",Colors.Aqua),new Tag("Solo",Colors.Red),new Tag("Eazy",Colors.Green) }),
                    new GameInfo("TestGame4","Unaware of loss. ",0,root+"\\4\\game\\STG.exe",root+"\\4\\panel\\icon.png",new string[]{ root + "\\4\\panel\\img1.png" },"",new GameController[]{GameController.Xbox},new Tag[0]),
                    new GameInfo("TestGame5","Nor aware of gain. ",0,root+"\\1\\game\\DOUTONBORI.exe",root+"\\1\\panel\\icon.png",new string[]{ root + "\\1\\panel\\img1.png" },"",new GameController[0], new Tag[0]),
                    new GameInfo("TestGame6","Withstood pain to create weapons, ",0,root+"\\2\\game\\STG.exe",root+"\\2\\panel\\icon.png",new string[]{ root + "\\2\\panel\\img1.png" },"",new GameController[0], new Tag[0]),
                    new GameInfo("TestGame7","waiting for one's arrival. ",0,root+"\\3\\game\\DOUTONBORI.exe",root+"\\3\\panel\\icon.png",new string[]{ root + "\\3\\panel\\img1.png" },"",new GameController[0], new Tag[0]),
                    new GameInfo("TestGame8","I have no regrets.This is the only path. ",0,root+"\\4\\game\\STG.exe",root+"\\4\\panel\\icon.png",new string[]{ root + "\\4\\panel\\img1.png" },"",new GameController[0], new Tag[0]),
                    new GameInfo("TestGame9","My whole life was",0,root+"\\1\\game\\DOUTONBORI.exe",root+"\\1\\panel\\icon.png",new string[]{ root + "\\3\\panel\\img1.png" },"",new GameController[0], new Tag[0]),
                    new GameInfo("TestGame10","unlimited blade works",0,root+"\\2\\game\\STG.exe",root+"\\2\\panel\\icon.png",new string[]{ root + "\\4\\panel\\img1.png" },"",new GameController[0], new Tag[0]),
                });
            iGamesDataConnector =moq.Object;
        }

        //パネルの作成。ViewModelもここで抽入する
        public void Launch(StackPanel parent_panel){
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
