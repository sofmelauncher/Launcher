using meGaton.src.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace meGaton.src.Views
{
    /// <summary>
    /// CategoryTag.xaml の相互作用ロジック
    /// </summary>
    public partial class CategoryTag : UserControl
    {
        public CategoryTag()
        {
            InitializeComponent();
            ((FrameworkElement)this.Content).DataContext = new CategoryTagViewModel();
        }

        public void SetValue(string name, Color color) {
            (((FrameworkElement)this.Content).DataContext as CategoryTagViewModel).SetTag(name,color);
        }
    }
}
