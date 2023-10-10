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

public class RootUndestructable : BaseMonobehaviour
{
    [SerializeField] private SceneContext _context;
    [BoxGroup("Scriptable objects")]
    [SerializeField] private StartSettings _startSettings;

    private Root _root;
    
    private IResourceLoader _resourceLoader;
    private CharacterPm _character;
    private Inventory _inventory;
    private Profile _profile;
    private SelectorPm _selector;
    private MainHUDPm _hud;
    private List<SoilPm> _soilPms = new List<SoilPm>();
    private List<JuicerPm> _juicerPms = new List<JuicerPm>();
    private ProductionGeneratorFactory _productionGeneratorFactoryPm;
    private ItemDataFactory _itemDataFactory;
    private PurchaseDispatcher _purchaseDispatcher;
    private ReactiveEvent<Purchase> _purchaseEvent;
    private ReactiveEvent<SelectorInfo> _selectorEvent;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        _root = new Root(new Root.Ctx
        {
            settings = _startSettings,
            sceneContext = _context
        });
    }

    

    protected override void OnDestroy()
    {
        _root?.Dispose();
        base.OnDestroy();
    }
}
