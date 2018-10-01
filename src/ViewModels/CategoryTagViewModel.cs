using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Input;
using Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using meGaton.DataResources;
using meGaton.Models;
using Reactive.Bindings;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Reactive.Bindings.Extensions;

namespace meGaton.src.ViewModels
{
    public class CategoryTagViewModel : INotifyPropertyChanged, IDisposable {
        public event PropertyChangedEventHandler PropertyChanged;//no use
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public ReactiveProperty<string> MyCategory { get; set;}
        public ReactiveProperty<Brush> MyColor{ get; set; }

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
