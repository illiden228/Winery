using System;
using Core;
using TMPro;
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
            public IReadOnlyReactiveProperty<int> moneys;
        }
        
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _storeButton;
        [SerializeField] private TMP_Text _moneysText;

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
            _ctx.moneys.Subscribe(count =>
            {
                _moneysText.text = count.ToString();
            }).AddTo(_ctx.viewDisposables);
        }
       
    }
}