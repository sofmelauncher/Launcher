using System.Linq;
using System.Windows;
using System.Windows.Controls;
using meGaton.ViewModels;
using meGaton.Views;
using MaterialDesignThemes.Wpf;

namespace meGaton.Models
{
    class PanelCreater {


        public PanelCreater() {
        }


        public void Launch(StackPanel parent_panel) {
            foreach (var i in Enumerable.Range(0,10)){

                parent_panel.Children.Add(CreateObject(i));
            }
        }

        private UIElement CreateObject(int i=0) {
            var element=new GamePanel(new GamePanelViewModel(i.ToString()));
            element.DataContext = i.ToString();
            return element;
        }

    }
}
