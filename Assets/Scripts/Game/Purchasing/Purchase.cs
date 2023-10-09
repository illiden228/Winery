using System;
using Core;
using Data;

namespace Game.Purchasing
{
    public enum PurchaseType
    {
        Buy,
        Sell
    }
    
    public struct Purchase
    {
        public PurchaseType PurchaseType;
        public Action Callback;
        public Item Item;

        public Purchase(Item item, PurchaseType type = PurchaseType.Buy, Action callback = null)
        {
            Item = item;
            PurchaseType = type;
            Callback = callback;
        }
    }
}