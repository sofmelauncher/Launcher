using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            var element = new Card();
            element.Padding =  new Thickness(48);
            element.Margin=new Thickness(8);
            ShadowAssist.GetShadowDepth(element);
            element.Content = "label" + i.ToString();
            return element;
        }
        /*
         *             <materialDesign:Card
                materialDesign:ShadowAssist.ShadowDepth="Depth1"
                Padding="32">
                DEPTH 1
            </materialDesign:Card>
         */
    }
}
