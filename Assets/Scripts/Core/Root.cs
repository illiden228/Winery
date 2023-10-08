using System.Collections.Generic;
using Core;
using Data;
using Game.Character;
using Game.Player;
using Game.Selectables;
using Tools;
using Game.Factories;
using Game.Purchasing;
using Tools.Extensions;
using UI;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class Root : BaseMonobehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Camera _camera;
    [SerializeField] private List<SoilView> _soils;    
    [SerializeField] private PlantCatalog _plantCatalog;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private StartSettings _startSettings;
    private IResourceLoader _resourceLoader;
    private CharacterPm _character;
    private Inventory _inventory;
    private Profile _profile;
    private MainHUDPm _hud;
    private List<SoilPm> _soilPms = new List<SoilPm>();
    private PlantFactory _plantFactoryPm;
    private PurchaseDispatcher _purchaseDispatcher;
    private ReactiveEvent<Purchase> _purchaseEvent;

    private void Awake()
    {
        _purchaseEvent = new ReactiveEvent<Purchase>();

        _resourceLoader = new ResourcePreLoader(new ResourcePreLoader.Ctx
        {
            maxLoadDelay = 0f,
            minLoadDelay = 0f
        });

        CharacterPm.Ctx characterCtx = new CharacterPm.Ctx
        {
            resourceLoader = _resourceLoader,
            startPosition = _startPosition.position,
            camera = _camera,
            startSpeed = _startSettings.CharacterSpeed
        };
        _character = new CharacterPm(characterCtx);

        _plantFactoryPm = new PlantFactory(new PlantFactory.Ctx
        {
            plantCatalog = _plantCatalog
        });

        List<SeedlingData> seedlingDatas = new List<SeedlingData>();
        foreach (var plantAsset in _startSettings.StartPlants)
        {
            seedlingDatas.Add(new SeedlingData
            {
                Plant = plantAsset
            });
        }
        
        Inventory.Ctx inventoryCtx = new Inventory.Ctx
        {
            startSeedlings = seedlingDatas
        };
        _inventory = new Inventory(inventoryCtx);

        Profile.Ctx profileCtx = new Profile.Ctx
        {
            inventory = _inventory,
            moneys = _startSettings.StartMoneys
        };
        _profile = new Profile(profileCtx);

        PurchaseDispatcher.Ctx purchaseDispatcherCtx = new PurchaseDispatcher.Ctx
        {
            inventory = _inventory,
            profile = _profile,
            purchaseEvent = _purchaseEvent
        };
        _purchaseDispatcher = new PurchaseDispatcher(purchaseDispatcherCtx);

        MainHUDPm.Ctx hudCtx = new MainHUDPm.Ctx
        {
            resourceLoader = _resourceLoader,
            mainCanvas = _mainCanvas,
            profile = _profile,
            purchaseEvent = _purchaseEvent,
        };
        _hud = new MainHUDPm(hudCtx);

        int id = 0;
        foreach (var soil in _soils)
        {
            _soilPms.Add(CreateSoilPm(soil, id++));
        }
    }

    private SoilPm CreateSoilPm(SoilView view, int id)
    {
        return new SoilPm(new SoilPm.Ctx
        {
            view = view,
            plantFactory = _plantFactoryPm,
            id = id,
            inventory = _inventory
        });
    }

    protected override void OnDestroy()
    {
        foreach (var soilPm in _soilPms)
        {
            soilPm.Dispose();
        }
        _resourceLoader?.Dispose();
        _character?.Dispose();
        _inventory?.Dispose();
        _profile?.Dispose();
        _hud?.Dispose();
        _plantFactoryPm.Dispose();
        base.OnDestroy();
    }
}
