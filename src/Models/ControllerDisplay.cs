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

namespace meGaton.Models{
    /// <summary>
    /// 使用するゲームのコントローラを明暗で表示する
    /// </summary>
    public class ControllerDisplay :INotifyPropertyChanged, IDisposable {
        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        private readonly IconInfo[] iconInfos;

        //各アイコンにバインドされる
        public List<ReactiveProperty<Brush>> ColorList { get; private set; }

        //使用可否のカラー
        private readonly Brush ACTIVE_COLOR = new SolidColorBrush(Colors.Black);
        private readonly Brush NON_ACTIVE_COLOR = new SolidColorBrush(Color.FromArgb(30,0,0,0));


        public ControllerDisplay(Panel root) {
            //IconInfoを作成
            iconInfos = new IconInfo[] {
                new IconInfo(GameController.Xbox,"GoogleController"),
                new IconInfo(GameController.Mouse,"Mouse"),
                new IconInfo(GameController.Keyboard,"Keyboard"),
            };
            //アイコンrootに配置されているコントローラアイコンの数だけバインド用プロパティを用意
            ColorList = Enumerable
                .Range(0, root.Children.OfType<UIElement>().Count(n => n is PackIcon))
                .Select(_ => new ReactiveProperty<Brush>().AddTo(Disposable))
                .ToList();

            //一度色を初期化
            foreach (var item in ColorList) {
                item.Value = NON_ACTIVE_COLOR;
            }

            if (iconInfos.Length != ColorList.Count){
                Logger.Inst.Log("Number of define icon is different from number of controller icon",LogLevel.Error);
                return;
            }

            SetIconsIndex(root.Children.OfType<UIElement>());
        }

        //IconInfoとコントローラアイコンを比較してColorListに対応するインデックスをセットする
        private void SetIconsIndex(IEnumerable<UIElement> icons) {
            var counter = 0;
            foreach (var item in icons) {
                var temp = item as PackIcon;
                if (temp == null){
                    continue;
                }

                var icon_p = iconInfos.Find(n => n.kind == temp.Kind.ToString());

                icon_p.index = counter;
                counter++;
            }
        }

        //アイコンの色を切り替える
        public void ChangeIcon(GameController[] game_controllers) {
            if (game_controllers == null){
                throw new ArgumentException();
            }

            //先頭のアイコンから順に対応するIconInfoを探し、引数に含まれていた場合はアクティブカラーに変更する
            for (var i = 0; i < ColorList.Count; i++) {
                var target_color = ColorList[i];
                var target_correspondence = iconInfos.Find(n => n.index == i);
                if (target_correspondence == null) {
                    Logger.Inst.Log(@"Index Error by Correspondence Settings.");
                    continue;
                }
                target_color.Value = game_controllers.Any(n => n == target_correspondence.me)
                    ? ACTIVE_COLOR
                    : NON_ACTIVE_COLOR;
            }
        }
        
        public void Dispose() {
            Disposable.Dispose();
        }

        //GameController列挙型と実際に使用されているPackIconの名前、バインドされているカラーを紐づける
        //わざわざIndexと名前を使って紐づけを行っているのはViewのコントローラアイコンの並び順に依存しないため
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
