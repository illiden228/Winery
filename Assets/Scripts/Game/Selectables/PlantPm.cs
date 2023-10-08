using Core;
using System;
using System.Diagnostics;
using Tools.Extensions;
using UnityEngine;

public class PlantPm : BaseDisposable, IGrowable
{
    public struct Ctx
    {
        public PlantView plantView;
        public PlantAsset asset;
    }

    private readonly Ctx _ctx;
    private int _currentStage;
    private IDisposable _updateGrowthDisposable;

    public bool Grown => _currentStage == _ctx.asset.StageCount;

    public PlantPm(Ctx ctx)
    {
        _ctx = ctx;
        _ctx.plantView.Init(new PlantView.Ctx { growthTime = _ctx.asset.GrowthTime });
        _currentStage = 1;
        _ctx.plantView.UpdatePlantView(_currentStage);
        _updateGrowthDisposable = ReactiveExtensions.DelayedCall(_ctx.asset.GrowthTime / _ctx.asset.StageCount, UpdateGrowth);
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
        _updateGrowthDisposable = ReactiveExtensions.DelayedCall(_ctx.asset.GrowthTime / _ctx.asset.StageCount, UpdateGrowth);
    }

    protected override void OnDispose()
    {
        _ctx.plantView.DestroyView();

        base.OnDispose();
    }
}