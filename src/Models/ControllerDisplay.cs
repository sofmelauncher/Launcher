using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Castle.Core.Internal;
using meGaton.DataResources;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace meGaton.Models
{
    public class ControllerDisplay :INotifyPropertyChanged, IDisposable {

        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        private readonly IconInfo[] _iconInfos;

        public List<ReactiveProperty<Brush>> ColorList { get; private set; }

        
        private readonly Brush ACTIVE_COLOR = new SolidColorBrush(Colors.Black);
        private readonly Brush NON_ACTIVE_COLOR = new SolidColorBrush(Color.FromArgb(30,0,0,0));

        public ControllerDisplay(StackPanel root) {
            _iconInfos = new IconInfo[] {
                new IconInfo(GameController.Xbox,"GoogleController"),
                new IconInfo(GameController.Mouse,"Mouse"),
                new IconInfo(GameController.Keyboard,"Keyboard"),
            };

            ColorList = Enumerable
                .Range(0, root.Children.OfType<UIElement>().Count(n => n is PackIcon))
                .Select(_ => new ReactiveProperty<Brush>().AddTo(Disposable))
                .ToList();
            foreach (var item in ColorList){
                item.Value = NON_ACTIVE_COLOR;
            }

            SetIconPara(root.Children.OfType<UIElement>());
        }

        private void SetIconPara(IEnumerable<UIElement> icons) {
            var counter = 0;
            foreach (var item in icons) {
                var temp = item as PackIcon;
                if (temp == null){
                    Logger.Inst.Log(item+ "isn't PackIcon.Plz don't include anything other than PackIcon",LogLevel.Warning);
                    continue;
                }

                var icon_p = _iconInfos.Find(n => n.kind == temp.Kind.ToString());

                icon_p.index = counter;
                counter++;
            }
        }

        public void ChangeIcon(GameController[] game_controllers) {
            if (ColorList.Count != _iconInfos.Length) {
                Logger.Inst.Log(@"The lengths of ColorList and Icon do not match");
                return;
            }

            
            for (var i = 0; i < ColorList.Count; i++) {
                var target_color = ColorList[i];
                var target_correspondence = _iconInfos.Find(n => n.index == i);
                if (target_correspondence == null) {
                    Logger.Inst.Log(@"Index Error by Correspondence Settings.");
                    continue;
                }
                target_color.Value = game_controllers != null&&game_controllers.Any(n => n == target_correspondence.me)
                    ? ACTIVE_COLOR
                    : NON_ACTIVE_COLOR;
            }
        }
        
        public void Dispose() {
            Disposable.Dispose();
        }


        private class IconInfo {
            public GameController me;
            public string kind;
            public int index;

            public IconInfo(GameController me, string type_name, int index = -1) {
                this.me = me;
                this.kind = type_name;
                this.index = index;
            }
        }
    }
}
