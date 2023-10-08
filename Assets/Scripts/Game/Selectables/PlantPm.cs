using Core;
using System;
using Data;
using Tools.Extensions;
using UnityEngine;
using Factories;

public class PlantPm : BaseDisposable, IGrowable
{
    public struct Ctx
    {
        public PlantView plantView;
        public SeedlingData seedling;
        public ItemDataFactory itemDataFactory;
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
    public bool Grown => _currentStep == _ctx.seedling.SproutStageCount;
    public bool Ripened => _currentStep == _ctx.seedling.RipeStageCount && _currentPlantStage == PlantStages.FruitsRipened;

    public PlantPm(Ctx ctx)
    {
        _ctx = ctx;
        _ctx.plantView.Init(new PlantView.Ctx { growthTime = _ctx.seedling.GrowthTime, fruitRipeTime = _ctx.seedling.FruitRipeTime });
        _currentPlantStage = PlantStages.Growing;
        _currentStep = 1;
        ctx.plantView.UpdatePlantView(_currentStep);
        _updateGrowthDisposable = ReactiveExtensions.RepeatableDelayedCall(_ctx.seedling.GrowthTime / _ctx.seedling.SproutStageCount, UpdateGrowth);
    }

    public void UpdateGrowth()
    {
        //To do: rewrite to state machine

        switch (_currentPlantStage)
        {
            case PlantStages.Growing:
                {
                    _currentStep++;
                    _ctx.plantView.UpdatePlantView(_currentStep, Grown);

                    if (Grown)
                    {
                        _currentStep = 0;
                        _currentPlantStage = PlantStages.FruitsRipening;

                        if (_updateGrowthDisposable != null)
                            _updateGrowthDisposable.Dispose();

                        _updateGrowthDisposable = ReactiveExtensions.RepeatableDelayedCall(_ctx.seedling.FruitRipeTime / _ctx.seedling.RipeStageCount, UpdateGrowth);
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
                    _ctx.plantView.UpdatePlantView(_currentStep, Ripened);                    

                    if (_currentStep == _ctx.seedling.RipeStageCount)
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

                    _updateGrowthDisposable = ReactiveExtensions.RepeatableDelayedCall(_ctx.seedling.FruitRipeTime / _ctx.seedling.RipeStageCount, UpdateGrowth);
                    break;
                }
        }
    }

    public void PickUpFruits(Action<Item> callBack)
    {
        if (_currentPlantStage != PlantStages.FruitsRipened)
            return;

        UpdateGrowth();

        callBack?.Invoke(_ctx.itemDataFactory.CreateObject(_ctx.seedling.Production, 1));
        _ctx.plantView.UpdatePlantView(0);
        Debug.Log("Плоды собраны!");
    }

    protected override void OnDispose()
    {
        if (_updateGrowthDisposable != null)
            _updateGrowthDisposable.Dispose();

        _ctx.plantView.DestroyView();

        base.OnDispose();
    }
}


