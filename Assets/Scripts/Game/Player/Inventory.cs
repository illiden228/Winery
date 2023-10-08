using System.Collections.Generic;
using Core;
using Data;
using UniRx;

namespace Game.Player
{
    public class Inventory : BaseDisposable, IInventory
    {
        public struct Ctx
        {
            public ReactiveCollection<SeedlingData> startSeedlings;
        }

        private readonly Ctx _ctx;

        public Inventory(Ctx ctx)
        {
            _ctx = ctx;
        }


        public IReadOnlyCollection<SeedlingData> Seedlings => _ctx.startSeedlings;
    }
}