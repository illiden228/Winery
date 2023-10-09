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
            public ReactiveCollection<Item> startItems;
        }

        private readonly Ctx _ctx;
        private ReactiveCollection<Item> _seedlings;
        private ReactiveCollection<Item> _grapes;
        private ReactiveCollection<Item> _juice;
        private ReactiveCollection<Item> _wine;
        private ReactiveCollection<Item> _allItems;
        
        public IReadOnlyCollection<Item> Seedlings => _seedlings;
        public IReadOnlyCollection<Item> Grapes => _grapes;      
        public IReadOnlyCollection<Item> AllItems => _allItems;
        public IReadOnlyCollection<Item> Juice => _juice;
        public IReadOnlyCollection<Item> Wine => _wine;

        public IReadOnlyCollection<Item> GetItemsWithType<T>() where T : Item => GetItemsWithType(typeof(T));
        
        public Inventory(Ctx ctx)
        {
            _ctx = ctx;
            _seedlings = new ReactiveCollection<Item>();
            _grapes = new ReactiveCollection<Item>();
            _juice = new ReactiveCollection<Item>();
            _wine = new ReactiveCollection<Item>();
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
            if (type.Equals(typeof(GrapeData)))
                return _grapes;
            if (type.Equals(typeof(JuiceData)))
                return _grapes;
            if (type.Equals(typeof(WineData)))
                return _grapes;

            Debug.LogError($"Imposible find collection for type {type.Name}");
            return null;
        }

        public void AddItemToInventory(Item newItem)
        {
            foreach (var item in _allItems)
            {
                if (item.TryAdd(newItem.Id, newItem.Count))
                {
                    return;
                }
            }
            SwitchAndAddItem(newItem);
        }
        
        public void RemoveFromInventory(Item newItem)
        {
            foreach (var item in _allItems)
            {
                if (item.TryRemove(newItem.Id, newItem.Count))
                {
                    return;
                }
            }
            SwitchAndRemoveItem(newItem);
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
                case JuiceData:
                    _juice.Add((JuiceData)item);
                    break;
                case WineData:
                    _wine.Add((WineData)item);
                    break;
            }
        }
        
        private void SwitchAndRemoveItem(Item item)
        {
            _allItems.Remove(item);
            switch (item)
            {
                case SeedlingData:
                    _seedlings.Remove(item);
                    break;
                case GrapeData:
                    _grapes.Remove((GrapeData) item);
                    break;
                case JuiceData:
                    _juice.Remove((JuiceData)item);
                    break;
                case WineData:
                    _wine.Remove((WineData)item);
                    break;
            }
        }
    }
}