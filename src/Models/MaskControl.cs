using System;
using System.Windows;
using System.Windows.Controls;
using meGaton.Views;

namespace meGaton.Models {
    /// <summary>
    /// 実行中のマスク表示の制御を行います
    /// </summary>
    public class MaskControl {
        
        private UIElement currentMask;
        private readonly Panel root;

        
        /// <param name="root">アプリケーションウィンドウのルート</param>
        public MaskControl(Panel root) {
            this.root = root;
        }

        /// <summary>
        /// マスクを表示します
        /// </summary>
        public void Run() {
            currentMask=new RunningMask();
            root.Children.Add(currentMask);
        }

        
        /// <summary>
        /// マスクを非表示にします
        /// </summary>
        public void Remove() {
            root.Dispatcher.BeginInvoke(new Action(() => {
                root.Children.Remove(currentMask);
                currentMask = null;
            }));

        }
    }
}
