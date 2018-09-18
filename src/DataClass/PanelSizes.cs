using System.Reactive.Disposables;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace meGaton.DataClass {
    public class PanelSizes {
        public ReactiveProperty<float> PanelWidth { get; }
        public ReactiveProperty<float> PanelHeight { get; }
        public ReactiveProperty<float> FontSize { get; }
        public ReactiveProperty<float> IconSize { get; }

        public float largeScale { private get;set; }

        private bool isLarge;

        public PanelSizes(CompositeDisposable disposable ,float panel_width,float panel_height,int font_size,float icon_size,float scale) {
            this.PanelWidth = new ReactiveProperty<float>().AddTo(disposable);
            this.PanelHeight = new ReactiveProperty<float>().AddTo(disposable);
            this.FontSize = new ReactiveProperty<float>().AddTo(disposable);
            this.IconSize = new ReactiveProperty<float>().AddTo(disposable);

            PanelWidth.Value = panel_width;
            PanelHeight.Value = panel_height;
            FontSize.Value = font_size;
            IconSize.Value = icon_size;

            largeScale = scale;
        }

        public void Enlarge() {
            if(isLarge)return;
            PanelWidth.Value *=largeScale;
            PanelHeight.Value *= largeScale;
            FontSize.Value *= largeScale;
            IconSize.Value *= largeScale;
            isLarge = true;
        }

        public void Undo() {
            if (!isLarge) return;
            PanelWidth.Value /= largeScale;
            PanelHeight.Value /= largeScale;
            FontSize.Value /= largeScale;
            IconSize.Value /= largeScale;
            isLarge = false;
        }
    }
}