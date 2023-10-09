using System.Collections.Generic;
using Core;
using Data;
using Factories;
using Game.Factories;
using Game.Selectables;
using UniRx;

namespace Core
{
    public class StartSettingsLoader : BaseDisposable
    {
        public struct Ctx
        {
            public StartSettings settings;
            public ItemDataFactory itemDataFactory;
        }

        private readonly Ctx _ctx;

        public ReactiveCollection<Item> StartInventory => GetStartInventory();
        public ReactiveCollection<Item> StartStock => GetStartStock();
        public int StartMoneys => GetStartMoneys();

        public StartSettingsLoader(Ctx ctx)
        {
            _ctx = ctx;
        }

        private ReactiveCollection<Item> GetStartInventory()
        {
            ReactiveCollection<Item> startInventory = new ReactiveCollection<Item>();
            foreach (var asset in _ctx.settings.StartPlants)
            {
                startInventory.Add(_ctx.itemDataFactory.CreateObject(asset));
            }

            return startInventory;
        }

        private ReactiveCollection<Item> GetStartStock()
        {
            ReactiveCollection<Item> stock = new ReactiveCollection<Item>();
            foreach (var asset in _ctx.settings.StartStock)
            {
                stock.Add(_ctx.itemDataFactory.CreateObject(asset));
            }

            return stock;
        }

        private int GetStartMoneys()
        {
            return _ctx.settings.StartMoneys;
        }
    }
}