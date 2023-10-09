using System;
using System.Collections.Generic;
using Data;
using UniRx;

namespace Game.Player
{
    public interface IInventory
    {
        public IReadOnlyReactiveCollection<Item> Seedlings { get; }
        public IReadOnlyReactiveCollection<Item> Grapes { get; }
        public IReadOnlyReactiveCollection<Item> Juice { get; }
        public IReadOnlyReactiveCollection<Item> Wine { get; }
        public IReadOnlyReactiveCollection<Item> AllItems { get; }
        public IReadOnlyReactiveCollection<Item> GetItemsWithType<T>() where T : Item;
        public IReadOnlyReactiveCollection<Item> GetItemsWithType(Type type);
    }
}