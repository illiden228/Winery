using System;
using Core;
using Data;
using Game.Player;
using Tools.Extensions;
using UniRx;

namespace Game.Purchasing
{
    public class PurchaseDispatcher : BaseDisposable
    {
        public struct Ctx
        {
            public Profile profile;
            public Inventory inventory;
            public ReactiveEvent<Purchase> purchaseEvent;
        }

        private readonly Ctx _ctx;

        public PurchaseDispatcher(Ctx ctx)
        {
            _ctx = ctx;
            AddDispose(_ctx.purchaseEvent.SubscribeWithSkip(OnPurchase));
        }

        private void OnPurchase(Purchase purchase)
        {
            Item item = purchase.Item;
            if(purchase.PurchaseType == PurchaseType.Buy)
                if (_ctx.profile.TryRemoveMoney(item.Cost))
                {
                    purchase.Callback?.Invoke();
                    _ctx.inventory.AddItemToInventory(item, item.Count);
                }
            if(purchase.PurchaseType == PurchaseType.Sell)
                _ctx.profile.AddMoneys(item.Cost * item.Count); 
            
            purchase.Callback?.Invoke();
        }
    }
}