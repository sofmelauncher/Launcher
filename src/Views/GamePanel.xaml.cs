using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using meGaton.ViewModels;

namespace meGaton.Views
{
    /// <summary>
    /// GamePanel.xaml の相互作用ロジック
    /// </summary>
    public partial class GamePanel : UserControl {

        private Storyboard upSlideAnimation;

        public GamePanel(GamePanelViewModel game_panel_view_model) {
            InitializeComponent();
            game_panel_view_model.SetPanelSizes(600,120,32,100,1.2f);
            ((FrameworkElement) this.Content).DataContext = game_panel_view_model;
        }

        private void control_Loaded(object sender, RoutedEventArgs e) {

        }
    }
}
