using System;
using Core;
using Data;
using UnityEngine;

namespace Game.Selectables
{
    public class CarView : BaseMonobehaviour, ISelectable
    {
        public struct Ctx
        {
            public Action<Item> onSelect;
            public Func<SelectableStatus> getStatus;
        }

        private Ctx _ctx;
        [SerializeField] private float _offset;

        public float Offset => _offset;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void Activate(Item item)
        {
            _ctx.onSelect?.Invoke(item);
        }

        public SelectableStatus GetSelectState()
        {
            return _ctx.getStatus?.Invoke();
        }
    }
}