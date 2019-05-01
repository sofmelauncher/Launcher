using System.Reactive.Disposables;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace meGaton.Models {
    /// <summary>
    /// パネルの拡大縮小を行う
    /// </summary>
    public class PanelSizes {

        private readonly float largeScale;
        private bool isLarge;

        
        /// <summary>現在のスケール</summary>
        public ReactiveProperty<double> MyScale { get;}
        
        
        /// <param name="disposable">親ViewModelのDispose</param>
        /// <param name="scale">拡大値</param>
        public PanelSizes(CompositeDisposable disposable ,float scale) {
            MyScale = new ReactiveProperty<double>().AddTo(disposable);
            
            largeScale = scale;
            MyScale.Value = 1.0f;
        }

        /// <summary>
        /// ステートフルな拡大
        /// </summary>
        public void Enlarge() {
            if(isLarge) return;
            MyScale.Value = largeScale;
            isLarge = true;
        }

        /// <summary>
        /// ステートフルな縮小
        /// </summary>
        public void Undo() {
            if (!isLarge) return;
            MyScale.Value = 1;
            isLarge = false;
        }

        /// <summary>
        /// ステートレスな可変
        /// </summary>
        public void Launch(){
            if (isLarge){
                Undo();
            }else{
                Enlarge();
            }
        }
    }
}