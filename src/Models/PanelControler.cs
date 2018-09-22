﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using meGaton.ViewModels;

namespace meGaton.src.Models {
    public class PanelControler {
        private StackPanel panelParent;

        private const int focusIndex=2;

        public PanelControler(StackPanel stack_panel) {
            panelParent = stack_panel;
            FocusPanel();
        }

        private GamePanelViewModel GetViewModel(int index) {
            var fe = (FrameworkElement)((UserControl)panelParent.Children[index]).Content;
           return (GamePanelViewModel)fe.DataContext;
        }

        private void FocusPanel() {
              GetViewModel(focusIndex).PanelSizes.Enlarge();
        }

        private void UnFocusPanel() {
            GetViewModel(focusIndex).PanelSizes.Undo();
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
