using System;
using Core;
using Data;

namespace Game.Selectables
{
    public class BarrelView : BaseMonobehaviour, ISelectable
    {
        public struct Ctx
        {
            public Action<Item> onSelect;
            public Func<SelectableStatus> getStatus;
        }

        private Ctx _cxt;

        public void Init(Ctx ctx)
        {
            _cxt = ctx;
        }

        public void Activate(Item item)
        {
            _cxt.onSelect?.Invoke(item);
        }

        public SelectableStatus GetSelectState()
        {
            return _cxt.getStatus?.Invoke();
        }
    }
}