using System.Collections.Generic;
using Core;
using Game.Character;
using Game.Selectables;
using Tools;
using Unity.VisualScripting;
using UnityEngine;

public class Root : BaseMonobehaviour
{
    [SerializeField] private Transform _startPositon;
    [SerializeField] private Camera _camera;
    [SerializeField] private List<SoilView> _soils;
    private IResourceLoader _resourceLoader;
    private CharacterPm _character;
    private List<SoilPm> _soilPms = new List<SoilPm>();
    
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
        base.OnDestroy();
    }
}
