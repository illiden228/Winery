using Core;
using Data;
using Game.Character;
using UnityEngine;
using Game.Factories;
using Game.Player;
using Factories;

namespace Game.Selectables
{
    public class SoilPm : ProdutionGenerator
    {
        public struct Ctx
        {
            public ProductionGeneratorFactory ProductionGeneratorFactory;
            public Inventory inventory;
        }

        private readonly Ctx _ctx;
        private PlantPm _currentSeedling;
        private SoilView _view;
        private SeedlingData _seedlingData;

        public SoilPm(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void InitView(SoilView view)
        {
            _view = view;
            _view.Init(new SoilView.Ctx
            {
                onSelect = OnSelect,
                getStatus = OnGetSelectStatus
            });
        }
        
        public override void StartGeneration(Item to, Item from = null)
        {
            if (_view == null)
            {
                Debug.LogError("View is missing");
                return;
            }
            
            _seedlingData = (SeedlingData)to;
        }

        private SelectableStatus OnGetSelectStatus()
        {
            if (_currentSeedling == null)
                return new SelectableStatus
                {
                    NeedSelector = true,
                    AnimationTriggerName = CharacterAnimation.Triggers.Take,
                    RequiredTypeForSelector = typeof(SeedlingData)
                };
            if(_currentSeedling.Ripened)
                return new SelectableStatus
                {
                    NeedSelector = false,
                    AnimationTriggerName = CharacterAnimation.Triggers.Collect
                };

            return null;
        }

        private void OnSelect(Item item)
        {
            StartGeneration(item);
            if (_currentSeedling == null)
            {
                SeedlingData seedling;
                if(item is SeedlingData)
                    seedling = item as SeedlingData;
                else
                    return;
                _currentSeedling = (PlantPm)_ctx.ProductionGeneratorFactory.CreateObject(new GrapeData());
                _currentSeedling.InitView(_view.transform);
                _currentSeedling.StartGeneration(item, null);
                _ctx.inventory.RemoveFromInventory(item, 1);
                Debug.Log($"Выбрана грядка посажен росток {seedling.Name}!");
            }
            else
            {
                if (_currentSeedling.Ripened)
                    _currentSeedling.PickUpFruits((Item) =>
                    {
                        GrapeData data = Item as GrapeData;
                        _ctx.inventory.AddItemToInventory(data, data.ProductionCount);
                    });
            }
        }
    }
}
