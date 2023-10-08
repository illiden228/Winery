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
            foreach (var seedling in _seedlings)
            {
                if (seedling.TryAdd(seedlingData.Plant.Id, seedlingData.Count))
                {
                    return;
                }
            }
            _seedlings.Add(seedlingData);
        }

        public void RemoveFromInventory(SeedlingData seedlingData)
        {
            foreach (var seedling in _seedlings)
            {
                if (seedling.TryRemove(seedlingData.Plant.Id, seedlingData.Count))
                {
                    return;
                }
            }
            _seedlings.Remove(seedlingData);
        }
    }
}