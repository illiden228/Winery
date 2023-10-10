using Core;
using System;
using Data;
using Game.Selectables;
using UnityEngine;

public class JuicerView : BaseMonobehaviour, ISelectable
{
    public struct Ctx
    {
        public Action<Item> onSelect;
        public Func<SelectableStatus> getStatus;
    }

    private Ctx _cxt;
    [SerializeField] private float _offset;

    public float Offset => _offset;

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
