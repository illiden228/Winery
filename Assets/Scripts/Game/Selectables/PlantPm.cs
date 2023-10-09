using Core;
using System;
using Data;
using Tools.Extensions;
using UnityEngine;
using Factories;
using Game.Selectables;
using Tools;

public class PlantPm : ProdutionGenerator, IGrowable
{
    public struct Ctx
    {
        public ItemDataFactory itemDataFactory;
        public IResourceLoader resourceLoader;
    }

    public enum PlantStages
    {
        Growing,
        WaitingWater,
        FruitsRipening,
        FruitsRipened
    }

    private readonly Ctx _ctx;
    private int _currentStep;
    private IDisposable _updateGrowthDisposable;
    private PlantStages _currentPlantStage;
    private SeedlingData _seedling;
    private GrapeData _production;
    private PlantView _view;
    private Transform _parent;
    public bool Grown => _currentStep == _seedling.SproutStageCount;
    public bool Ripened => _currentStep == _seedling.RipeStageCount && _currentPlantStage == PlantStages.FruitsRipened;

    public PlantPm(Ctx ctx)
    {
        _ctx = ctx;
    }

    public void InitView(Transform parent)
    {
        _parent = parent;
    }
    
    public override void StartGeneration(Item to, Item from)
    {
        if(to != null)
            _seedling = (SeedlingData)to;
        if(from != null)
            _production = (GrapeData)from;

        AddDispose(_ctx.resourceLoader.LoadPrefab("fake", _seedling.ViewPrefabName, OnViewLoaded));
    }

    private void OnViewLoaded(GameObject viewPrefab)
    {
        _view = GameObject.Instantiate(viewPrefab, _parent).GetComponent<PlantView>();
        _view.Init(new PlantView.Ctx
        {
            growthTime = _seedling.GrowthTime, 
            fruitRipeTime = _seedling.FruitRipeTime
        });
        _currentPlantStage = PlantStages.Growing;
        _currentStep = 1;
        _view.UpdatePlantView(_currentStep);
        _updateGrowthDisposable = ReactiveExtensions.RepeatableDelayedCall(_seedling.GrowthTime / _seedling.SproutStageCount, UpdateGrowth);
    }

    public void UpdateGrowth()
    {
        //To do: rewrite to state machine

        switch (_currentPlantStage)
        {
            case PlantStages.Growing:
                {
                    _currentStep++;
                    _view.UpdatePlantView(_currentStep, Grown);

                    if (Grown)
                    {
                        _currentStep = 0;
                        _currentPlantStage = PlantStages.FruitsRipening;

                        if (_updateGrowthDisposable != null)
                            _updateGrowthDisposable.Dispose();

                        _updateGrowthDisposable = ReactiveExtensions.RepeatableDelayedCall(_seedling.FruitRipeTime / _seedling.RipeStageCount, UpdateGrowth);
                    }

                    break;
                }
            case PlantStages.WaitingWater:
                {
                    break;
                }
            case PlantStages.FruitsRipening:
                {
                    _currentStep++;
                    _view.UpdatePlantView(_currentStep, Ripened);                    

                    if (_currentStep == _seedling.RipeStageCount)
                    {
                        if (_updateGrowthDisposable != null)
                            _updateGrowthDisposable.Dispose();

                        _currentPlantStage = PlantStages.FruitsRipened;
                    }

                    break;
                }
            case PlantStages.FruitsRipened:
                {
                    _currentStep = 0;
                    _currentPlantStage = PlantStages.FruitsRipening;

                    _updateGrowthDisposable = ReactiveExtensions.RepeatableDelayedCall(_seedling.FruitRipeTime / _seedling.RipeStageCount, UpdateGrowth);
                    break;
                }
        }
    }

    public void PickUpFruits(Action<Item> callBack)
    {
        if (_currentPlantStage != PlantStages.FruitsRipened)
            return;

        UpdateGrowth();

        callBack?.Invoke(_ctx.itemDataFactory.CreateObject(_seedling.Production));
        _view.UpdatePlantView(0);
        Debug.Log("Плоды собраны!");
    }

    protected override void OnDispose()
    {
        if (_updateGrowthDisposable != null)
            _updateGrowthDisposable.Dispose();

        _view.DestroyView();

        base.OnDispose();
    }
}


