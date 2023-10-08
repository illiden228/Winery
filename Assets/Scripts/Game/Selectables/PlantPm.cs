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

    private readonly Ctx _ctx;
    private float _currentGrowthTime;
    private int _currentStage;
    private IDisposable _updateGrowthDisposable;

    public bool Grown => _currentStage == _ctx.seedling.Plant.StageCount;

    public PlantPm(Ctx ctx)
    {
        _ctx = ctx;
        _ctx.plantView.Init(new PlantView.Ctx { growthTime = _ctx.seedling.Plant.GrowthTime });
        _currentStage = 1;
        _ctx.plantView.UpdatePlantView(_currentStage);
        _updateGrowthDisposable = ReactiveExtensions.DelayedCall(_ctx.seedling.Plant.GrowthTime / _ctx.seedling.Plant.StageCount, UpdateGrowth);
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
        _updateGrowthDisposable = ReactiveExtensions.DelayedCall(_ctx.seedling.Plant.GrowthTime / _ctx.seedling.Plant.StageCount, UpdateGrowth);
    }

    protected override void OnDispose()
    {
        _ctx.plantView.DestroyView();

        base.OnDispose();
    }
}