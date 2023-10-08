using Core;
using Tools.Extensions;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlantPm : BaseDisposable, IGrowable
{
    public struct Ctx
    {
        public PlantView plantView;
        public PlantAsset asset;
    }

    private readonly Ctx _ctx;
    private float _currentGrowthTime;

    public bool Grown => _currentGrowthTime >= _ctx.asset.GrowthTime;

    public PlantPm(Ctx ctx)
    {
        _ctx = ctx;
        _ctx.plantView.Init(new PlantView.Ctx { growthTime = _ctx.asset.GrowthTime }, _ctx.asset.GetGrowthStagePrefabs());

        ReactiveExtensions.StartUpdate(UpdateGrowth);
    }

    public void UpdateGrowth()
    {
        if (Grown)
            return;

        _currentGrowthTime += Time.deltaTime;
        _ctx.plantView.UpdatePlantView(_currentGrowthTime, Grown);
    }
}