using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using meGaton.Models;
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

            var panel_creater = new PanelCreater();
            panel_creater.Launch(window.FindName("PanelParent") as StackPanel);

            var view_model = new MainViewModel(window.FindName("PanelParent") as StackPanel);

            window.DataContext = view_model;

           
            window.Show();
        }
    }
}
