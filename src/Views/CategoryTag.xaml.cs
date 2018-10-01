using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using meGaton.src.ViewModels;

namespace meGaton.Views
{
    /// <summary>
    /// CategoryTag.xaml の相互作用ロジック
    /// </summary>
    public partial class CategoryTag : UserControl
    {
        public CategoryTag(string name, Color color)
        {
            InitializeComponent();
            ((FrameworkElement)this.Content).DataContext = new CategoryTagViewModel(name,color);
        }
    }
}
