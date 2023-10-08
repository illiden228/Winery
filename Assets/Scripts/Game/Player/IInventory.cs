using System.Collections.Generic;
using Data;

namespace Game.Player
{
    public interface IInventory
    {
        public IReadOnlyCollection<SeedlingData> Seedlings { get; }
    }
}