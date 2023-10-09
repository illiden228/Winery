using System;
using Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class MainHUDView : BaseMonobehaviour
    {
        public struct Ctx
        {
            public CompositeDisposable viewDisposables;
            public Action inventoryButtonClick; 
            public Action storeButtonClick; 
        }
        
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _storeButton;

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            _inventoryButton.OnClickAsObservable()
                .Subscribe(_ => _ctx.inventoryButtonClick?.Invoke())
                .AddTo(_ctx.viewDisposables);
            _storeButton.OnClickAsObservable()
                .Subscribe(_ => _ctx.storeButtonClick?.Invoke())
                .AddTo(_ctx.viewDisposables);
        }
       
    }
}