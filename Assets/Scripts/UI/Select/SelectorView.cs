using Core;
using UniRx;
using UnityEngine;

namespace UI.Select
{
    public class SelectorView : BaseMonobehaviour
    {
        public struct Ctx
        {
            public CompositeDisposable viewDisposables;
            public IReadOnlyReactiveProperty<bool> open;
        }

        [SerializeField] private Transform _container;
        
        private Ctx _ctx;

        public Transform Container => _container;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            _ctx.open.Subscribe(open => gameObject.SetActive(open)).AddTo(_ctx.viewDisposables);
        }
    }
}