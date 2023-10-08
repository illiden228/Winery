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
            SeedlingData seedlingData = purchase.SeedlingData;
            if(_ctx.profile.TryRemoveMoney(seedlingData.Plant.Cost))
            {
                purchase.Callback?.Invoke();
                _ctx.inventory.AddItemToInventory(seedlingData);
            }
        }
    }
}