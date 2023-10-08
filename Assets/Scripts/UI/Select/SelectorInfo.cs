using System;
using Core;
using Data;
using UniRx;

namespace UI.Select
{
    public class SelectorInfo
    {
        public bool Open;
        public ReactiveProperty<Item> Item;
    }
}