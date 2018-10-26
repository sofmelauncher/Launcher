using System;
using System.Data;
using System.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using meGaton.DataResources;
using meGaton.ViewModels;

namespace meGaton.Models {
    public class PanelController {
        public GameInfo GetCurrentPanelsInfo => GetViewModel(FOCUS_INDEX).MyGameInfo;
        private readonly StackPanel panelParent;

        private readonly Subject<GameInfo> changeSelectedSubject = new Subject<GameInfo>();
        public IObservable<GameInfo> ChangeSelectedPanel => changeSelectedSubject;

        private Random randomer;
        private readonly int FOCUS_INDEX;

        public PanelController(StackPanel stack_panel) {
            panelParent = stack_panel;
            FOCUS_INDEX = stack_panel.Children.Count > 2 ? 2 : 0;

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
            if (index > panelParent.Children.Count){
                Logger.Inst.Log("Argument is more than GamePanels number",LogLevel.Error);
                throw new ArgumentException();
            }
            var fe = (FrameworkElement)((UserControl)panelParent.Children[index]).Content;
            var res = fe.DataContext as GamePanelViewModel;
            if (res == null){
                Logger.Inst.Log("Plz don't include anything other than GamePanel in PanelParent",LogLevel.Error);
                throw new DataException();
            }
            return res;
        }

        private void FocusPanel(){
            GamePanelViewModel c=null;
            try {
                c = GetViewModel(FOCUS_INDEX);
            } catch (Exception e){
                try{
                    c = GetViewModel(0);
                }
                catch (Exception exception){
                    Logger.Inst.Log("I didn't focus panel bc PanelParent don't has GamePanel");
                    return;
                }
            }
            c.PanelSizes.Enlarge();
            changeSelectedSubject.OnNext(c.MyGameInfo);

        }

        private void UnFocusPanel() {
            GamePanelViewModel c = null;
            try {
                c = GetViewModel(FOCUS_INDEX);
            } catch (Exception e) {
                try {
                    c = GetViewModel(0);
                } catch (Exception exception) {
                    Logger.Inst.Log("I didn't un focus panel bc PanelParent don't has GamePanel");
                    return;
                }
            }
            c.PanelSizes.Undo();
        }


    }
}
