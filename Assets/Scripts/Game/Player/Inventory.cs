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
            public List<SeedlingData> startSeedlings;
        }

        private readonly Ctx _ctx;
        private ReactiveCollection<SeedlingData> _seedlings;
        public IReadOnlyCollection<SeedlingData> Seedlings => _seedlings;

        public Inventory(Ctx ctx)
        {
            _ctx = ctx;
            _seedlings = new ReactiveCollection<SeedlingData>(_ctx.startSeedlings);
        }

        public void AddItemToInventory(SeedlingData seedlingData)
        {
            _seedlings.Add(seedlingData);
        }

        public void RemoveFromInventory(SeedlingData seedlingData)
        {
            _seedlings.Remove(seedlingData);
        }
    }
}