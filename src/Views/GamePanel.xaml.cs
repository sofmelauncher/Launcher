using System.Windows;
using System.Windows.Controls;
using meGaton.ViewModels;
using System.Windows.Input;

namespace meGaton.Views{
    /// <summary>
    /// GamePanel.xaml の相互作用ロジック
    /// </summary>
    public partial class GamePanel : UserControl {

        //動的生成のみであるためVieModelはファクトリに注入される
        public GamePanel(GamePanelViewModel game_panel_view_model) {
            InitializeComponent();

            game_panel_view_model.SetPanelLargeScale(1.2f);//拡大時の倍率はViewが責任を持つべきなのでこちらで設定する

            ((FrameworkElement) this.Content).DataContext = game_panel_view_model;
            CreateTags();
        }

        //View全域に対するクリックイベントはViewでしか拾えないため、処理はViewModelに委譲
        private void MouseClick(object sender,MouseButtonEventArgs e) {
            (((FrameworkElement)this.Content).DataContext as GamePanelViewModel)?.MouseClickSubmit();
        }

        //タグ生成
        //子要素のViewを作るためViewのメソッドで行っているがなんか違う気がするので下の層に移したい
        private void CreateTags() {
            var tags = (((FrameworkElement)this.Content).DataContext as GamePanelViewModel)?.MyGameInfo.Tags;//DataContextへのアクセスが美しくない
            var root = this.FindName("TagParent") as Panel;
            if (tags==null||root == null) return;
            foreach (var item in tags) {
                var temp = new CategoryTag(new CategoryTagViewModel(item.Category, item.BgColor));//ViewModelは抽入する
                root.Children.Add(temp);
            }
        }

    }
}
