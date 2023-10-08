using System.Collections.Generic;
using Core;
using Data;
using UniRx;

namespace Game.Player
{
    public interface IReadOnlyProfile
    {
        public IInventory Inventory { get; }
        public IReadOnlyReactiveProperty<int> Moneys { get; }
    }
}