using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using meGaton.ViewModels;
using meGaton.Views;

namespace meGaton {
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var window = new MainView();
            var view_model = new MainViewModel();

            window.DataContext = view_model;
            window.Show();
        }
    }
}
