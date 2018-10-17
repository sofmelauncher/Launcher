﻿using System.Reactive.Disposables;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace meGaton.Models {
    public class PanelSizes {
        public ReactiveProperty<double> MyScale { get;}
       
        public float largeScale { private get;set; }

        private bool isLarge;

        public PanelSizes(CompositeDisposable disposable ,float scale) {
            this.MyScale = new ReactiveProperty<double>().AddTo(disposable);
            
            largeScale = scale;
            MyScale.Value = 1.0f;
        }

        public void Enlarge() {
            if(isLarge)return;
            MyScale.Value = largeScale;
            isLarge = true;
        }

        public void Undo() {
            if (!isLarge) return;
            MyScale.Value = 1;
            isLarge = false;
        }
    }
}