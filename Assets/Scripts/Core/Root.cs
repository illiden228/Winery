using System.Collections.Generic;
using Core;
using Data;
using Factories;
using Game.Character;
using Game.Factories;
using Game.Player;
using Game.Purchasing;
using Game.Selectables;
using Tools;
using Tools.Extensions;
using UI.HUD;
using UI.Select;
using UniRx;

namespace Core
{
    public class Root : BaseDisposable
    {
        public struct Ctx
        {
            public StartSettings settings;
            public SceneContext sceneContext;
        }

        private readonly Ctx _ctx;
        private ReactiveCollection<SoilPm> _soils;
        private ReactiveCollection<JuicerPm> _juicers;
        private ReactiveCollection<BarrelPm> _barrels;
        private CarPm _car;

        public Root(Ctx ctx)
        {
            _ctx = ctx; 
            ReactiveEvent<Purchase> purchaseEvent = AddDispose(new ReactiveEvent<Purchase>());
            ReactiveEvent<SelectorInfo> selectorEvent = AddDispose(new ReactiveEvent<SelectorInfo>());

            IResourceLoader resourceLoader = AddDispose(new ResourcePreLoader(new ResourcePreLoader.Ctx
            {
                maxLoadDelay = 0f,
                minLoadDelay = 0f
            }));
        
            ItemDataFactory itemDataFactory = AddDispose(new ItemDataFactory(new ItemDataFactory.Ctx
            {   
                plantCatalog = _ctx.settings.PlantCatalog,
                grapeCatalog = _ctx.settings.GrapeCatalog,
                juiceCatalog = _ctx.settings.JuiceCatalog,
                wineCatalog = _ctx.settings.WineCatalog
            }));
            
            StartSettingsLoader startSettingsLoader = AddDispose(new StartSettingsLoader(new StartSettingsLoader.Ctx
            {
                settings = _ctx.settings,
                itemDataFactory = itemDataFactory,
            }));
            
            Inventory inventory = AddDispose(new Inventory(new Inventory.Ctx
            {
                startItems = startSettingsLoader.StartInventory,
            }));

            Profile profile = AddDispose(new Profile(new Profile.Ctx
            {
                inventory = inventory,
                moneys = startSettingsLoader.StartMoneys
            }));
            
            ProductionGeneratorFactory productionGeneratorFactory = AddDispose(new ProductionGeneratorFactory(new ProductionGeneratorFactory.Ctx
            {
                resourceLoader = resourceLoader,
                itemDataFactory = itemDataFactory,
                inventory = inventory
            }));

            StartContextLoader startContextLoader = AddDispose(new StartContextLoader(new StartContextLoader.Ctx
            {
                inventory = inventory,
                purchaseEvent = purchaseEvent,
                ProductionGeneratorFactory = productionGeneratorFactory,
                startSoils = _ctx.sceneContext.Soils,
                startJuicers = _ctx.sceneContext.Juicers,
                startBarrels = _ctx.sceneContext.Barrels,
                startCar = _ctx.sceneContext.Car
            }));

            CharacterPm character = AddDispose(new CharacterPm(new CharacterPm.Ctx
            {
                resourceLoader = resourceLoader,
                startPosition = _ctx.sceneContext.StartPosition.position,
                camera = _ctx.sceneContext.Camera,
                startSpeed = _ctx.settings.CharacterSpeed,
                selectorEvent = selectorEvent,
                eventSystem = _ctx.sceneContext.EventSystem
            }));

            PurchaseDispatcher purchaseDispatcher = AddDispose(new PurchaseDispatcher(new PurchaseDispatcher.Ctx
            {
                inventory = inventory,
                profile = profile,
                purchaseEvent = purchaseEvent
            }));

            MainHUDPm mainHUD = AddDispose(new MainHUDPm(new MainHUDPm.Ctx
            {
                resourceLoader = resourceLoader,
                mainCanvas = _ctx.sceneContext.MainCanvas,
                profile = profile,
                purchaseEvent = purchaseEvent,
                stock = startSettingsLoader.StartStock,
                itemDataFactory = itemDataFactory
            }));

            SelectorPm selector = AddDispose(new SelectorPm(new SelectorPm.Ctx
            {
                inventory = inventory,
                mainCanvas = _ctx.sceneContext.MainCanvas,
                resourceLoader = resourceLoader,
                selectorEvent = selectorEvent
            }));

            _soils = startContextLoader.StartSoils;
            _juicers = startContextLoader.StartJuicers;
            _barrels = startContextLoader.StartBarrels;
            _car = startContextLoader.Car;
        }

        protected override void OnDispose()
        {
            for (int i = 0; i < _soils.Count; i++)
            {
                _soils[i].Dispose();
            }
            _soils.Dispose();
            
            for (int i = 0; i < _juicers.Count; i++)
            {
                _juicers[i].Dispose();
            }
            _juicers.Dispose();
            
            for (int i = 0; i < _barrels.Count; i++)
            {
                _barrels[i].Dispose();
            }
            _barrels.Dispose();
            
            _car.Dispose();
            base.OnDispose();
        }
    }
}