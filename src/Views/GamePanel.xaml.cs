using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using meGaton.ViewModels;
using System;
using meGaton.src.Views;
using System.Windows.Media;

namespace meGaton.Views
{
    /// <summary>
    /// GamePanel.xaml の相互作用ロジック
    /// </summary>
    public partial class GamePanel : UserControl {

        public GamePanel(GamePanelViewModel game_panel_view_model) {
            InitializeComponent();
            game_panel_view_model.SetPanelSizes(1.2f);
            ((FrameworkElement) this.Content).DataContext = game_panel_view_model;
            CreateTags();
            //game_panel_view_model.ButtonNotification.Subscribe(n =>);
        }

        private void CreateTags() {
            var tags = (((FrameworkElement)this.Content).DataContext as GamePanelViewModel).MyGameInfo.Categorys;
            var root = this.FindName("TagParent") as Grid;
            var row = root.RowDefinitions.Count;
            var column = root.ColumnDefinitions.Count;
            Console.WriteLine(row + ","+column);

            var color_order=new Color[]{Colors.OrangeRed,Colors.Aqua,Colors.Green,Colors.Yellow,Colors.YellowGreen};
            var counter = 0;
            for (var i = 0; i < row; i++) {
                for (var j = 0; j < column;j++) {
                    if (counter >=tags.Length) return;
                    var temp = new CategoryTag();
                    root.Children.Add(temp);
                    temp.SetValue(tags[counter],color_order[counter]);
                    Grid.SetRow(temp, i);
                    Grid.SetColumn(temp, j);
                    counter++;
                }
            }
        }

    }
}
