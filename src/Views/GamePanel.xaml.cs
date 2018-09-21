using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using meGaton.ViewModels;
using System;

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

            //game_panel_view_model.ButtonNotification.Subscribe(n =>);
        }

    }
}
