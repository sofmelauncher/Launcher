using System.Windows;
using System.Windows.Controls;
using meGaton.ViewModels;

namespace meGaton.Views{
    /// <summary>
    /// CategoryTag.xaml の相互作用ロジック
    /// </summary>
    public partial class CategoryTag : UserControl{

        //動的生成される前提なのでViewModelは生成しない
        public CategoryTag(CategoryTagViewModel category_tag_view_model){
            InitializeComponent();
            ((FrameworkElement)this.Content).DataContext = category_tag_view_model;
        }
    }
}
