using System.Collections.Generic;
using Core;
using Data;

namespace Game.Player
{
    public interface IReadOnlyProfile
    {
        public IInventory Inventory { get; }
    }
}