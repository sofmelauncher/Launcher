using System;
using System.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using meGaton.DataResources;
using meGaton.ViewModels;

namespace meGaton.Models {
    public class PanelController {
        public GameInfo GetCurrentPanelsInfo => GetViewModel(FocusIndex).MyGameInfo;
        private readonly StackPanel panelParent;

        private readonly Subject<GameInfo> changeSelectedSubject = new Subject<GameInfo>();
        public IObservable<GameInfo> ChangeSelectedPanel => changeSelectedSubject;

        private Random randomer;
        private const int FocusIndex=2;

        public PanelController(StackPanel stack_panel) {
            panelParent = stack_panel;
            FocusPanel();

            randomer = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
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
            var end_point = panelParent.Children.Count - 1;
            var el = panelParent.Children[end_point];
            panelParent.Children.RemoveAt(end_point);
            panelParent.Children.Insert(0, el);
            FocusPanel();
        }

        public void Shuffle() {
            Action func;
            if (randomer.Next(0, 2) == 0) {
                func = SlideDown;
            } else {
                func = SlideUp;
            }

            foreach (var nouse in Enumerable.Range(0, randomer.Next(panelParent.Children.Count)+1)) {
                func.Invoke();
            }

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

        
    }
}
