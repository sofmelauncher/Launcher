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
            Logger.Inst.Log("------------meGaton launch------------");
            base.OnStartup(e);

            var window = new MainView();

            window.Show();
        }

        ~App()
        {
            Logger.Inst.Log("------------meGaton Finish------------");
        }
    }
}
