using Core;
using NaughtyAttributes;
using System;
using Game.Character;
using Game.Selectables;
using UnityEngine;

public class SoilView : BaseMonobehaviour, ISelectable
{
    public struct Ctx
    {
        public Action onSelect;
    }

    private Ctx _cxt;

    public void Init(Ctx ctx)
    {
        _cxt = ctx;
    }
    
    public void Select()
    {
        _cxt.onSelect?.Invoke();
    }    
}
