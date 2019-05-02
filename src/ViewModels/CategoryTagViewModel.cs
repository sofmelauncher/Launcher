using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Media;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace meGaton.ViewModels{
    public class CategoryTagViewModel : INotifyPropertyChanged, IDisposable {
        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        //バインド用プロパティ
        public ReactiveProperty<string> MyCategory { get; set;}
        public ReactiveProperty<Brush> MyColor{ get; set; }

        /// <param name="category">タグ名</param>
        /// <param name="color">タグカラー</param>
        public CategoryTagViewModel(string category, Color color) {
            this.MyCategory = new ReactiveProperty<string>().AddTo(Disposable);
            this.MyColor = new ReactiveProperty<Brush>().AddTo(Disposable);

            this.MyCategory.Value = category;
            this.MyColor.Value = new SolidColorBrush(color);
        }

        public void Dispose() {
            Disposable.Dispose();
        }
    }
}
