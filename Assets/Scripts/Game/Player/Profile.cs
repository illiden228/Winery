using System.Collections.Generic;
using Core;
using Data;
using UniRx;

namespace Game.Player
{
    public class Profile : BaseDisposable, IReadOnlyProfile
    {
        public struct Ctx
        {
            public Inventory inventory;
            public int moneys;
        }

        private readonly Ctx _ctx;
        private ReactiveProperty<int> _moneys;
        
        public Profile(Ctx ctx)
        {
            _ctx = ctx;
            _moneys = new ReactiveProperty<int>(_ctx.moneys);
        }
        
        public IInventory Inventory => _ctx.inventory;
        public IReadOnlyReactiveProperty<int> Moneys => _moneys;

        public void AddMoneys(int value)
        {
            _moneys.Value += value;
        }

        public bool TryRemoveMoney(int count)
        {
            bool possible = count <= _moneys.Value;
            if (possible)
                _moneys.Value -= count;
            return possible;
        }
    }
}