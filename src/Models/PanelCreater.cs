using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using meGaton.DataResources;
using meGaton.ViewModels;
using meGaton.Views;
using MaterialDesignThemes.Wpf;
using Moq;

namespace meGaton.Models
{
    class PanelCreater {

        private GameProcessControll gameProcessControll;
        private readonly IGamesDataConnector iGamesDataConnector;

        public PanelCreater() {
            gameProcessControll=new GameProcessControll();

            var root = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')+ "\\Games";
            var moq = new Mock<IGamesDataConnector>();
            moq.Setup(m => m.GetGamesInfo())
                .Returns(new List<GameInfo>(){
                    new GameInfo("TestGame1","",0,root+"\\1\\game\\DOUTONBORI.exe",root+"\\1\\panel\\icon.png",new string[]{"\\1\\panel\\img1.png"}),
                    new GameInfo("TestGame2","",0,root+"\\2\\game\\STG.exe",root+"\\2\\panel\\icon.png",new string[]{"\\2\\panel\\img1.png"}),
                    new GameInfo("TestGame3","",0,root+"\\3\\game\\DOUTONBORI.exe",root+"\\3\\panel\\icon.png",new string[]{"\\3\\panel\\img1.png"}),
                    new GameInfo("TestGame4","",0,root+"\\4\\game\\STG.exe",root+"\\4\\panel\\icon.png",new string[]{"\\4\\panel\\img1.png"}),
                    new GameInfo("TestGame5","",0,root+"\\1\\game\\DOUTONBORI.exe",root+"\\1\\panel\\icon.png",new string[]{"\\1\\panel\\img1.png"}),
                    new GameInfo("TestGame6","",0,root+"\\2\\game\\STG.exe",root+"\\2\\panel\\icon.png",new string[]{"\\2\\panel\\img1.png"}),
                    new GameInfo("TestGame7","",0,root+"\\3\\game\\DOUTONBORI.exe",root+"\\3\\panel\\icon.png",new string[]{"\\3\\panel\\img1.png"}),
                    new GameInfo("TestGame8","",0,root+"\\4\\game\\STG.exe",root+"\\4\\panel\\icon.png",new string[]{"\\4\\panel\\img1.png"}),
                });
            iGamesDataConnector =moq.Object;
        }


        public void Launch(StackPanel parent_panel) {
            foreach (var item in iGamesDataConnector.GetGamesInfo()){
                parent_panel.Children.Add(CreateObject(item));
            }
        }

        private UIElement CreateObject(GameInfo game_info) {
            var element=new GamePanel(new GamePanelViewModel(game_info,gameProcessControll));
            return element;
        }

    }
}
