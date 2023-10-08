using Core;
using System;
using System.Diagnostics;
using Data;
using Tools.Extensions;
using UnityEngine;

public class PlantPm : BaseDisposable, IGrowable
{
    public struct Ctx
    {
        public PlantView plantView;
        public SeedlingData seedling;
    }
    
    public enum PlantStages
    {
        Growing,
        WaitingWater,
        FruitsRipening,
        FruitsRipened
    }

    private readonly Ctx _ctx;
    private float _currentGrowthTime;
    private int _currentStage;
    private IDisposable _updateGrowthDisposable;
    private PlantStages _currentPlantStage;

    public bool Grown => _currentStage == _ctx.seedling.Plant.StageCount;

    public PlantPm(Ctx ctx)
    {
        _ctx = ctx;
        
        _ctx.plantView.Init(new PlantView.Ctx { growthTime = _ctx.seedling.Plant.GrowthTime });
        _currentPlantStage = PlantStages.Growing;
        _currentStage = 1;
        _ctx.plantView.UpdatePlantView(_currentStage);
        _updateGrowthDisposable = ReactiveExtensions.RepeatableDelayedCall(_ctx.seedling.Plant.GrowthTime / _ctx.seedling.Plant.StageCount, UpdateGrowth);
    }

    public void UpdateGrowth()
    {
        if (Grown)
        {
            if (_updateGrowthDisposable != null)
                _updateGrowthDisposable.Dispose();

            return;
        }

        _currentStage++;
        _ctx.plantView.UpdatePlantView(_currentStage, Grown);  
    }

    protected override void OnDispose()
    {
        _ctx.plantView.DestroyView();

        base.OnDispose();
    }
}