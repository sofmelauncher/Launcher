using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace meGaton.src.Models {
    public class PanelControler {
        private StackPanel panelParent;

        public PanelControler(StackPanel stack_panel) {
            panelParent = stack_panel;
        }

        public void SlidePanels() {
            var el = panelParent.Children[0];
            panelParent.Children.RemoveAt(0);
            panelParent.Children.Add(el);
        }
    }
}
