using System;
using System.Collections.Generic;
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
    public class ControllerDisplay:IDisposable{
        private class IconCorrespondence {
            public GameController me;
            public string kind;
            public int index;

            public IconCorrespondence(GameController me, string type_name,int index=-1) {
                this.me = me;
                this.kind = type_name;
                this.index = index;
            }
        }

        private IconCorrespondence[] iconCorrespondences;
        private List<UIElement> currentIcon=new List<UIElement>();
        public List<ReactiveProperty<Brush>> colorList { get; private set; }

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        private readonly Brush ACTIVE_COLOR = new SolidColorBrush(Colors.Black);
        private readonly Brush NON_ACTIVE_COLOR = new SolidColorBrush(Color.FromArgb(30,0,0,0));

        public ControllerDisplay(StackPanel root) {
            iconCorrespondences = new IconCorrespondence[] {
                new IconCorrespondence(GameController.Xbox,"GoogleController"),
                new IconCorrespondence(GameController.Mouse,"Mouse"),
                new IconCorrespondence(GameController.Keyboard,"Keyboard"),
            };
            colorList = Enumerable.Range(0, root.Children.OfType<UIElement>().Count(n => n is PackIcon))
                .Select(_ => new ReactiveProperty<Brush>().AddTo(Disposable)).ToList();

            SetIconPara(root.Children.OfType<UIElement>());
        }

        private void SetIconPara(IEnumerable<UIElement> icons) {
            var counter = 0;
            foreach (var item in icons) {
                var temp = item as PackIcon;
                if(temp==null)continue;

                var icon_p = iconCorrespondences.Find(n => n.kind == temp.Kind.ToString());

                icon_p.index = counter;
                counter++;
            }
        }

        public void ChangeIcon(GameController[] game_controllers) {
            if (colorList.Count != iconCorrespondences.Length) {
                Console.WriteLine(@"The lengths of A and B do not match");
                return;
            }

            for (var i = 0; i < colorList.Count; i++) {
                var target_color = colorList[i];
                var target_correspondence = iconCorrespondences.Find(n => n.index == i);
                if (target_correspondence == null) {
                    Console.WriteLine(@"Index Error by Correspondence Settings.");
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
    }
}
