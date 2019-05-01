using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using Systems.Systems;
using meGaton.DataResources;
using meGaton.ViewModels;
using Console = System.Console;

namespace meGaton.Models {
    public class PanelController {
        public GamePanelViewModel GetCurrentPanelsInfo => GetViewModel(focusIndex);
        private readonly Panel panelParent;

        private List<GamePanelViewModel> gameViewModels=new List<GamePanelViewModel>();
        private readonly Subject<GamePanelViewModel> changeSelectedSubject = new Subject<GamePanelViewModel>();
        public IObservable<GamePanelViewModel> ChangeSelectedPanel => changeSelectedSubject;
        private readonly Subject<Unit> panelClickStream=new Subject<Unit>();
        public IObservable<Unit> PanelClickEvent => panelClickStream;

        private Random randomer;

        private int focusIndex;
        private readonly int START_POINT;
        private const int ENABLE_PANEL=6;
        private readonly int END_POINT;

        public PanelController(Panel stack_panel) {
            panelParent = stack_panel;
            focusIndex = stack_panel.Children.Count > 2 ? 2 : 0;
            START_POINT = stack_panel.Children.Count > 1 ? 1 : 0;
            END_POINT = START_POINT + ENABLE_PANEL-1;

            randomer = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            foreach (var item in stack_panel.Children){
                try{
                    var temp = ((FrameworkElement)((UserControl)item).Content).DataContext as GamePanelViewModel;
                    gameViewModels.Add(temp);
                } catch (Exception e){
                    Logger.Inst.Log(e.ToString());
                    gameViewModels.Add(null);
                }
            }

            gameViewModels
                .Select(n => n.OnClickEvent)
                .Merge()
                .Where(n => gameViewModels.Skip(START_POINT).Take(ENABLE_PANEL).Contains(n))
                .Subscribe(n =>{
                    UnFocusPanel();
                    focusIndex = gameViewModels.FindIndex(x=>x==n);
                    FocusPanel();
                    panelClickStream.OnNext(Unit.Default);
                });

            FocusPanel();
        }

        public void MoveUp() {
            panelParent.Dispatcher.BeginInvoke(new Action(() =>
            {
                UnFocusPanel();
                focusIndex--;
                if (focusIndex < START_POINT){
                    SlideUp();
                    focusIndex = START_POINT;
                }
                FocusPanel();
            }));
        }

        private void SlideUp() {
            var end_point = panelParent.Children.Count - 1;
            var el = panelParent.Children[end_point];
            panelParent.Children.RemoveAt(end_point);
            panelParent.Children.Insert(0, el);
            gameViewModels.Slide(1);
        }

        public void MoveDown(){
            panelParent.Dispatcher.BeginInvoke(new Action(() =>{
                UnFocusPanel();
                focusIndex++;
                if (focusIndex > END_POINT){
                    SlideDown();
                    focusIndex = END_POINT;
                }
                FocusPanel();
            }));
        }

        private void SlideDown() {
            var el = panelParent.Children[0];
            panelParent.Children.RemoveAt(0);
            panelParent.Children.Add(el);
            gameViewModels.Slide(-1);
        }

        public void Shuffle() {
            Action func;
            if (randomer.Next(0, 2) == 0) {
                func = SlideDown;
            } else {
                func = SlideUp;
            }

            panelParent.Dispatcher.BeginInvoke(new Action(() => {
                UnFocusPanel();
                foreach (var nouse in Enumerable.Range(0, randomer.Next(panelParent.Children.Count) + 1)) {
                    func.Invoke();
                }
                FocusPanel();
            }));
        }

        public void Skip(int vec) {
            Action func=null;
            if (vec>0) {
                func = SlideDown;
            } else if(vec<0){
                func = SlideUp;
            }
            
            panelParent.Dispatcher.BeginInvoke(new Action(() => {
                UnFocusPanel();
                foreach (var nouse in Enumerable.Range(0,ENABLE_PANEL)) {
                func?.Invoke();
                }
                FocusPanel();
            }));
            
            }

        private GamePanelViewModel GetViewModel(int index) {
            if (index > panelParent.Children.Count){
                Logger.Inst.Log("Argument is more than GamePanels number",LogLevel.Error);
                throw new ArgumentException();
            }
            var res = gameViewModels[index];
            if (res == null){
                Logger.Inst.Log("Plz don't include anything other than GamePanel in PanelParent",LogLevel.Error);
                throw new DataException();
            }
            return res;
        }

        private void FocusPanel(){
            GamePanelViewModel c=null;
            try {
                c = GetViewModel(focusIndex);
            } catch (Exception e){
                try{
                    c = GetViewModel(0);
                }
                catch (Exception exception){
                    Logger.Inst.Log(exception+"I didn't focus panel bc PanelParent don't has GamePanel");
                    return;
                }
            }
            c.PanelSizes.Enlarge();
            changeSelectedSubject.OnNext(c);

        }

        private void UnFocusPanel() {
            GamePanelViewModel c = null;
            try {
                c = GetViewModel(focusIndex);
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
