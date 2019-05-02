using System.Windows;
using meGaton.Util;
using meGaton.Views;

namespace meGaton {
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application {
        //エントリポイント
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
