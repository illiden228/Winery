using System;
using System.Collections.Generic;
using Core;
using Data;
using UniRx;
using UnityEngine;

namespace Game.Player
{
    public class Inventory : BaseDisposable, IInventory
    {
        public struct Ctx
        {
            public List<Item> startItems;
        }

        private readonly Ctx _ctx;
        private ReactiveCollection<Item> _seedlings;
        private ReactiveCollection<Item> _grapes;
        private ReactiveCollection<Item> _allItems;
        
        public IReadOnlyCollection<Item> Seedlings => _seedlings;
        public IReadOnlyCollection<Item> Grapes => _grapes;
        public IReadOnlyCollection<Item> AllItems => _allItems;
        public IReadOnlyCollection<Item> GetItemsWithType<T>() where T : Item => GetItemsWithType(typeof(T));
        
        public Inventory(Ctx ctx)
        {
            _ctx = ctx;
            _seedlings = new ReactiveCollection<Item>();
            _grapes = new ReactiveCollection<Item>();
            _allItems = new ReactiveCollection<Item>();

            foreach (var item in _ctx.startItems)
            {
                SwitchAndAddItem(item);
            }
        }
        
        public IReadOnlyCollection<Item> GetItemsWithType(Type type)
        {
            if (type.Equals(typeof(SeedlingData)))
                return _seedlings;
            if(type.Equals(typeof(GrapeData)))
                return _grapes;

            Debug.LogError($"Imposible find collection for type {type.Name}");
            return null;
        }

        public void AddItemToInventory(Item item)
        {
            foreach (var seedling in _seedlings)
            {
                if (seedling.TryAdd(item.Id, item.Count))
                {
                    return;
                }
            }
            SwitchAndAddItem(item);
        }
        
        public void RemoveFromInventory(Item item)
        {
            foreach (var seedling in _seedlings)
            {
                if (seedling.TryRemove(item.Id, item.Count))
                {
                    return;
                }
            }
            SwitchAndAddItem(item);
        }

        private void SwitchAndAddItem(Item item)
        {
            _allItems.Add(item);
            switch (item)
            {
                case SeedlingData:
                    _seedlings.Add(item);
                    break;
                case GrapeData:
                    _grapes.Add((GrapeData) item);
                    break;
            }
        }
    }
}