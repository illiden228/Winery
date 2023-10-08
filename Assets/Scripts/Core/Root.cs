using System.Collections.Generic;
using Core;
using Data;
using Factories;
using Game.Character;
using Game.Player;
using Game.Selectables;
using Tools;
using Game.Factories;
using Game.Purchasing;
using Tools.Extensions;
using UI;
using UI.HUD;
using UI.Select;
using UniRx;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class Root : BaseMonobehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Camera _camera;
    [BoxGroup("Scene objects")]
    [SerializeField] private List<SoilView> _soils;
    [BoxGroup("Scene objects")]
    [SerializeField] private List<JuicerView> _juicers;
    [BoxGroup("Scriptable objects")]
    [SerializeField] private PlantCatalog _plantCatalog;
    [BoxGroup("Scriptable objects")]
    [SerializeField] private GrapeCatalog _grapeCatalog;
    [BoxGroup("Scriptable objects")]
    [SerializeField] private JuiceCatalog _juiceCatalog;
    [BoxGroup("Scriptable objects")]
    [SerializeField] private WineCatalog _wineCatalog;
    [BoxGroup("Scriptable objects")]
    [SerializeField] private StartSettings _startSettings;
    [SerializeField] private Canvas _mainCanvas;    
    [SerializeField] private EventSystem _eventSystem;
    private IResourceLoader _resourceLoader;
    private CharacterPm _character;
    private Inventory _inventory;
    private Profile _profile;
    private SelectorPm _selector;
    private MainHUDPm _hud;
    private List<SoilPm> _soilPms = new List<SoilPm>();
    private List<JuicerPm> _juicerPms = new List<JuicerPm>();
    private PlantFactory _plantFactoryPm;
    private ItemDataFactory _itemDataFactory;
    private PurchaseDispatcher _purchaseDispatcher;
    private ReactiveEvent<Purchase> _purchaseEvent;
    private ReactiveEvent<SelectorInfo> _selectorEvent;

    private void Awake()
    {
        _purchaseEvent = new ReactiveEvent<Purchase>();
        _selectorEvent = new ReactiveEvent<SelectorInfo>();
        
        _itemDataFactory = new ItemDataFactory(new ItemDataFactory.Ctx
        {
            plantCatalog = _plantCatalog,
            grapeCatalog = _grapeCatalog,
            juiceCatalog = _juiceCatalog,
            wineCatalog = _wineCatalog
        });

        _plantFactoryPm = new PlantFactory(new PlantFactory.Ctx
        {
            plantCatalog = _plantCatalog,
            itemDataFactory = _itemDataFactory
        });

        _resourceLoader = new ResourcePreLoader(new ResourcePreLoader.Ctx
        {
            maxLoadDelay = 0f,
            minLoadDelay = 0f
        });
                
        List<Item> startInventory = new List<Item>();
        foreach (var asset in _startSettings.StartPlants)
        {
            startInventory.Add(_itemDataFactory.CreateObject(asset, 1));
        }
        
        ReactiveCollection<Item> stock = new ReactiveCollection<Item>();
        foreach (var asset in _startSettings.StartStock)
        {
            stock.Add(_itemDataFactory.CreateObject(asset, 1));
        }
        
        Inventory.Ctx inventoryCtx = new Inventory.Ctx
        {
            startItems = startInventory
        };
        _inventory = new Inventory(inventoryCtx);

        Profile.Ctx profileCtx = new Profile.Ctx
        {
            inventory = _inventory,
            moneys = _startSettings.StartMoneys
        };
        _profile = new Profile(profileCtx);

        CharacterPm.Ctx characterCtx = new CharacterPm.Ctx
        {
            resourceLoader = _resourceLoader,
            startPosition = _startPosition.position,
            camera = _camera,
            startSpeed = _startSettings.CharacterSpeed,
            selectorEvent = _selectorEvent,
            eventSystem = _eventSystem
        };
        _character = new CharacterPm(characterCtx);

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
            stock = stock
        };
        _hud = new MainHUDPm(hudCtx);
        
        SelectorPm.Ctx selectorCtx = new SelectorPm.Ctx
        {
            inventory = _inventory,
            mainCanvas = _mainCanvas,
            resourceLoader = _resourceLoader,
            selectorEvent = _selectorEvent
        };
        _selector = new SelectorPm(selectorCtx);

        int id = 0;
        foreach (var soil in _soils)
        {
            _soilPms.Add(CreateSoilPm(soil, id++));
        }

        id = 0;
        foreach (var juicer in _juicers)
        {
            _juicerPms.Add(CreateJuicerPm(juicer, id++));
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

    private JuicerPm CreateJuicerPm(JuicerView view, int id)
    {
        return new JuicerPm(new JuicerPm.Ctx
        {
            view = view,            
            id = id,
            inventory = _inventory,
            itemDataFactory = _itemDataFactory
        });
    }

    protected override void OnDestroy()
    {
        foreach (var soilPm in _soilPms)
        {
            soilPm.Dispose();
        }
        foreach (var juicePm in _juicerPms)
        {
            juicePm.Dispose();
        }
        _resourceLoader?.Dispose();
        _character?.Dispose();
        _inventory?.Dispose();
        _profile?.Dispose();
        _hud?.Dispose();
        _selector?.Dispose();
        _plantFactoryPm?.Dispose();
        _itemDataFactory?.Dispose();
        base.OnDestroy();
    }
}
