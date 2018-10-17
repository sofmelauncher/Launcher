using meGaton.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using meGaton.Models;

namespace meGaton.Views
{
    /// <summary>
    /// MainView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window {

        public MainView(){
            InitializeComponent();

            var panel_creater = new PanelCreater();
            panel_creater.Launch(FindName("PanelParent") as StackPanel);
            new CustomerTimer(this);
            var a = GamePadObserver.GetInstance;

            var view_model = new MainViewModel(
                new PanelControler(FindName("PanelParent") as StackPanel)
                ,new MediaDisplay(FindName("DisplayVideo") as MediaElement)
                ,new ControllerDisplay(FindName("ControllIconParent") as StackPanel));

            this.DataContext = view_model;
        }
    }
}
