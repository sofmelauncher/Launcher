using meGaton.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using meGaton.Models;

namespace meGaton.Views
{
    /// <summary>
    /// MainView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            var panel_creater = new PanelCreater();
            panel_creater.Launch(FindName("PanelParent") as StackPanel);

            var view_model = new MainViewModel(
                FindName("PanelParent") as StackPanel
                ,FindName("DisplayVideo") as MediaElement
                ,FindName("ControllIconParent") as StackPanel);

            this.DataContext = view_model;
        }
    }
}
