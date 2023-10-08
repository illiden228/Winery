using System.Collections.Generic;
using Core;
using Data;
using Game.Character;
using Game.Player;
using Game.Selectables;
using Tools;
using Game.Factories;
using UI;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class Root : BaseMonobehaviour
{
    [SerializeField] private Transform _startPositon;
    [SerializeField] private Camera _camera;
    [SerializeField] private List<SoilView> _soils;    
    [SerializeField] private PlantCatalog _plantCatalog;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private List<SoilView> _soils;
    [SerializeField] private List<PlantAsset> _plants;
    private IResourceLoader _resourceLoader;
    private CharacterPm _character;
    private Inventory _inventory;
    private Profile _profile;
    private MainHUDPm _hud;
    private List<SoilPm> _soilPms = new List<SoilPm>();
    private PlantFactoryPm _plantFactoryPm;
    private FactoryView factoryView;

    private const string PlantFactoryName = "PlantFactory";

    private ReactiveCollection<SeedlingData> _seedlings;
    
    private void Awake()
    {
        _resourceLoader = new ResourcePreLoader(new ResourcePreLoader.Ctx
        {
            maxLoadDelay = 0f,
            minLoadDelay = 0f
        });

        CharacterPm.Ctx characterCtx = new CharacterPm.Ctx
        {
            resourceLoader = _resourceLoader,
            startPosition = _startPositon.position,
            camera = _camera
        };
        _character = new CharacterPm(characterCtx);

        factoryView = new GameObject(PlantFactoryName).AddComponent<FactoryView>();

        _plantFactoryPm = new PlantFactoryPm(new PlantFactoryPm.Ctx
        {
            plantCatalog = _plantCatalog,
            factoryView = factoryView
        });

        int id = 0;
        foreach (var soil in _soils)
        {
            _soilPms.Add(CreateSoilPm(soil, id++));
        }

        _seedlings = new ReactiveCollection<SeedlingData>();
        
        Inventory.Ctx inventoryCtx = new Inventory.Ctx
        {
            startSeedlings = _seedlings
        };
        _inventory = new Inventory(inventoryCtx);

        Profile.Ctx profileCtx = new Profile.Ctx
        {
            inventory = _inventory
        };
        _profile = new Profile(profileCtx);

        MainHUDPm.Ctx hudCtx = new MainHUDPm.Ctx
        {
            resourceLoader = _resourceLoader,
            mainCanvas = _mainCanvas,
            profile = _profile
        };
        _hud = new MainHUDPm(hudCtx);

        foreach (var plant in _plants)
        {
            _seedlings.Add(new SeedlingData
            {
                Plant = plant
            });
        }
    }

    private SoilPm CreateSoilPm(SoilView view, int id)
    {
        return new SoilPm(new SoilPm.Ctx
        {
            view = view,
            plantFactory= _plantFactoryPm,
            id = id            
        });
    }

    protected override void OnDestroy()
    {
        foreach (var soilPm in _soilPms)
        {
            soilPm.Dispose();
        }
        _resourceLoader.Dispose();
        _character.Dispose();
        _seedlings.Dispose();
        _inventory.Dispose();
        _profile.Dispose();
        _hud.Dispose();
        base.OnDestroy();
    }
}
