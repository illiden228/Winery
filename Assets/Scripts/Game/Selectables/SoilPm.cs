using Core;
using Data;
using Game.Character;
using UnityEngine;
using Game.Factories;
using Game.Player;
using Factories;

namespace Game.Selectables
{
    public class SoilPm : BaseDisposable
    {
        public struct Ctx
        {
            public SoilView view;
            public PlantFactory plantFactory;
            public ItemDataFactory itemDataFactory;
            public Inventory inventory;
            public int id;
        }

        public enum SoilState
        {
            Empty,
            Grows,
            GrownUp
        }

        public enum SoilAction
        {
            PlaceGrowable,
            WaterGrowable,
            Harvest
        }

        private readonly Ctx _ctx;
        private PlantPm _currentPm;

        public SoilPm(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.view.Init(new SoilView.Ctx
            {
                onSelect = OnSelect,
                getStatus = OnGetSelectStatus
            });
        }

        private SelectableStatus OnGetSelectStatus()
        {
            if (_currentPm == null)
                return new SelectableStatus
                {
                    NeedSelector = true,
                    AnimationTriggerName = CharacterAnimation.Triggers.Take,
                    RequiredTypeForSelector = typeof(SeedlingData)
                };
            if(_currentPm.Ripened)
                return new SelectableStatus
                {
                    NeedSelector = false,
                    AnimationTriggerName = CharacterAnimation.Triggers.Collect
                };

            return null;
        }

        private void OnSelect(Item item)
        {
            if (_currentPm == null)
            {
                SeedlingData seedling;
                if(item is SeedlingData)
                    seedling = item as SeedlingData;
                else
                    return;
                _currentPm = (_ctx.plantFactory.GetPlantPmCtxById(seedling, _ctx.view.transform));
                _ctx.inventory.RemoveFromInventory(item);
                Debug.Log($"Выбрана грядка {_ctx.id} и посажен росток {seedling.Name}!");
            }
            else
            {
                if (_currentPm.Ripened)
                    _currentPm.PickUpFruits((seedlingData) =>
                    {
                        _ctx.inventory.AddItemToInventory(seedlingData);
                    });
            }
        }
    }
}
