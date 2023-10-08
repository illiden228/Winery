using System;
using Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryView : BaseMonobehaviour
    {
        public struct Ctx
        {
            public CompositeDisposable viewDisposables;
            public IReadOnlyReactiveProperty<bool> open;
            public Action onCloseClick;
        }

        [SerializeField] private Button _closeButton;
        [SerializeField] private Transform _container;
        
        private Ctx _ctx;

        public Transform Container => _container;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            _ctx.open.Subscribe(open => gameObject.SetActive(open)).AddTo(_ctx.viewDisposables);
            _closeButton.OnClickAsObservable().Subscribe(_ => _ctx.onCloseClick?.Invoke()).AddTo(_ctx.viewDisposables);
        }
    }
}