using System;
using System.Windows;
using System.Windows.Controls;
using meGaton.src.Views;

namespace meGaton.Models {
    public class MaskControll {
        private UIElement currentMask;

        private Panel root;

        public MaskControll(Panel root_grid) {
            root = root_grid;
        }

        public void Run() {
            currentMask=new RunningMask();
            root.Children.Add(currentMask);
        }

        public void Remove() {
            root.Dispatcher.BeginInvoke(new Action(() => {
                root.Children.Remove(currentMask);
                currentMask = null;
            }));

        }
    }
}
