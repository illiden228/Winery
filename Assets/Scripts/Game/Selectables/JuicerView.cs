using Core;
using System;
using Data;
using Game.Selectables;

public class JuicerView : BaseMonobehaviour, ISelectable
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
