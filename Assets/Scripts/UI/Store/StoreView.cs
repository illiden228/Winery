using System;
using Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Store
{
    public class StoreView : BaseMonobehaviour
    {
        public struct Ctx
        {
            public CompositeDisposable viewDisposables;
            public IReadOnlyReactiveProperty<bool> open;
            public Action onCloseClick;
            public IReadOnlyReactiveProperty<int> moneys;
        }
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private Transform _container;
        [SerializeField] private TMP_Text _moneysText;

        private Ctx _ctx;
        
        public Transform Container => _container;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            
            _ctx.open.Subscribe(open => gameObject.SetActive(open)).AddTo(_ctx.viewDisposables);
            _closeButton.OnClickAsObservable().Subscribe(_ => _ctx.onCloseClick?.Invoke()).AddTo(_ctx.viewDisposables);
            _ctx.moneys.Subscribe(count =>
            {
                _moneysText.text = count.ToString();
            }).AddTo(_ctx.viewDisposables);
        }
    }
}