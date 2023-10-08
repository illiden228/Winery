using System;
using System.Collections.Generic;
using Data;

namespace Game.Player
{
    public interface IInventory
    {
        public IReadOnlyCollection<Item> Seedlings { get; }
        public IReadOnlyCollection<Item> Grapes { get; }
        public IReadOnlyCollection<Item> Juice { get; }
        public IReadOnlyCollection<Item> Wine { get; }
        public IReadOnlyCollection<Item> AllItems { get; }
        public IReadOnlyCollection<Item> GetItemsWithType<T>() where T : Item;
        public IReadOnlyCollection<Item> GetItemsWithType(Type type);
    }
}