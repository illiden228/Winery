using Core;
using System;
using Data;
using Game.Selectables;

public class SoilView : BaseMonobehaviour, ISelectable
{
    public struct Ctx
    {
        public Action<Item> onSelect;
    }

    private Ctx _cxt;

    public void Init(Ctx ctx)
    {
        _cxt = ctx;
    }

    public void Select(Item item)
    {
        _cxt.onSelect?.Invoke(item);
    }
}
