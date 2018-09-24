using System;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using meGaton.DataResources;
using meGaton.ViewModels;

namespace meGaton.Models {
    public class PanelControler {
        public GameInfo GetCurrentPanelsInfo => GetViewModel(FocusIndex).MyGameInfo;
        private readonly StackPanel panelParent;

        private readonly Subject<GameInfo> changeSelectedSubject = new Subject<GameInfo>();
        public IObservable<GameInfo> ChangeSelectedPanel => changeSelectedSubject;

        private const int FocusIndex=2;

        public PanelControler(StackPanel stack_panel) {
            panelParent = stack_panel;
            FocusPanel();
        }

        private GamePanelViewModel GetViewModel(int index) {
            var fe = (FrameworkElement)((UserControl)panelParent.Children[index]).Content;
           return (GamePanelViewModel)fe.DataContext;
        }

        private void FocusPanel() {
            var c = GetViewModel(FocusIndex);
            c.PanelSizes.Enlarge();
            changeSelectedSubject.OnNext(c.MyGameInfo);
        }

        private void UnFocusPanel() {
            GetViewModel(FocusIndex).PanelSizes.Undo();
        }

        public void SlideUp() {
            UnFocusPanel();
            var el = panelParent.Children[0];
            panelParent.Children.RemoveAt(0);
            panelParent.Children.Add(el);
            FocusPanel();
        }
        public void SlideDown() {
            UnFocusPanel();
            var end_point = panelParent.Children.Count-1;
            var el = panelParent.Children[end_point];
            panelParent.Children.RemoveAt(end_point);
            panelParent.Children.Insert(0,el);
            FocusPanel();
        }
    }
}
