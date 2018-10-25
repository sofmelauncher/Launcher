using System;
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
            try{
                var fe = (FrameworkElement)((UserControl)panelParent.Children[index]).Content;
                return (GamePanelViewModel)fe.DataContext;
            } catch (Exception e){
                Logger.Inst.Log("Get GamePanel Error",LogLevel.Error);
                throw;
            }
        }

        private void FocusPanel() {
            try{
                var c = GetViewModel(FOCUS_INDEX);
                c.PanelSizes.Enlarge();
                changeSelectedSubject.OnNext(c.MyGameInfo);
            } catch (Exception e){
                Logger.Inst.Log(e+"I didnt focus panel");
            }
            }

        private void UnFocusPanel() {
            try{
                GetViewModel(FOCUS_INDEX).PanelSizes.Undo();
            }
            catch (Exception e){
                Logger.Inst.Log(e+"I didnt unfocus panel");
                throw;
            }
        }

        
    }
}
