using System;
using Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainHUDView : BaseMonobehaviour
    {
        public struct Ctx
        {
            public CompositeDisposable viewDisposables;
            public Action inventoryButtonClick; 
        }
        
        [SerializeField] private Button _inventoryButton;

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            _inventoryButton.OnClickAsObservable()
                .Subscribe(_ => _ctx.inventoryButtonClick?.Invoke())
                .AddTo(_ctx.viewDisposables);
        }
       
    }
}