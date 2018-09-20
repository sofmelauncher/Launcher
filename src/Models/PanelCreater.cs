using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using meGaton.DataClass;
using meGaton.DataResources;
using meGaton.ViewModels;
using meGaton.Views;
using MaterialDesignThemes.Wpf;
using Moq;

namespace meGaton.Models
{
    class PanelCreater {

        private IGamesDataConnector iGamesDataConnector;

        public PanelCreater() {
            var moq = new Mock<IGamesDataConnector>();
            moq.Setup(m => m.GetGamesInfo())
                .Returns(new List<GameInfo>(){
                    new GameInfo("1","",0,"",new string[]{""}),
                    new GameInfo("2","",0,"",new string[]{""}),
                    new GameInfo("3","",0,"",new string[]{""}),
                    new GameInfo("4","",0,"",new string[]{""}),
                    new GameInfo("5","",0,"",new string[]{""}),
                });
            iGamesDataConnector =moq.Object;
        }


        public void Launch(StackPanel parent_panel) {
            foreach (var item in iGamesDataConnector.GetGamesInfo()){
                parent_panel.Children.Add(CreateObject(item));
            }
        }

        private UIElement CreateObject(GameInfo game_info) {
            var element=new GamePanel(new GamePanelViewModel(game_info));
            return element;
        }

    }
}
