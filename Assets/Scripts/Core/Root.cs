using System.Collections.Generic;
using Core;
using Game.Character;
using Game.Selectables;
using Tools;
using Game.Factories;
using UnityEngine;

public class Root : BaseMonobehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Camera _camera;
    [SerializeField] private List<SoilView> _soils;    
    [SerializeField] private PlantCatalog _plantCatalog;
    private IResourceLoader _resourceLoader;
    private CharacterPm _character;
    private List<SoilPm> _soilPms = new List<SoilPm>();
    private PlantFactory _plantFactoryPm;

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
            startPosition = _startPosition.position,
            camera = _camera
        };
        _character = new CharacterPm(characterCtx);

        _plantFactoryPm = new PlantFactory(new PlantFactory.Ctx
        {
            plantCatalog = _plantCatalog
        });

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
        _plantFactoryPm.Dispose();
        base.OnDestroy();
    }
}
