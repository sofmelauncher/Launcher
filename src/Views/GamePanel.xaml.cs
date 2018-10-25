using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using meGaton.ViewModels;
using System;
using System.Windows.Media;
using System.Windows.Input;

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
        }

        private void MouseClick(object sender,MouseButtonEventArgs e) {
            (((FrameworkElement)this.Content).DataContext as GamePanelViewModel).MouseClickSubmit();
        }

        private void CreateTags() {
            var tags = (((FrameworkElement)this.Content).DataContext as GamePanelViewModel)?.MyGameInfo.Tags;
            var root = this.FindName("TagParent") as WrapPanel;
            if (tags==null||root == null) return;
            foreach (var item in tags) {
                var temp = new CategoryTag(item.category, item.bgColor);
                root.Children.Add(temp);
            }
        }

    }
}
