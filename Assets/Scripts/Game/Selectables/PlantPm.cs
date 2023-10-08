using Core;
using System;
using Tools.Extensions;

public class PlantPm : BaseDisposable, IGrowable
{
    public struct Ctx
    {
        public PlantView plantView;
        public PlantAsset asset;
    }
    
    public enum PlantStages
    {
        Growing,
        WaitingWater,
        FruitsRipening,
        FruitsRipened
    }

    private readonly Ctx _ctx;
    private int _currentStage;
    private IDisposable _updateGrowthDisposable;
    private PlantStages _currentPlantStage;

    public bool Grown => _currentStage == _ctx.asset.StageCount;

    public PlantPm(Ctx ctx)
    {
        _ctx = ctx;
        _ctx.plantView.Init(new PlantView.Ctx { growthTime = _ctx.asset.GrowthTime });
        _currentPlantStage = PlantStages.Growing;
        _currentStage = 1;
        _ctx.plantView.UpdatePlantView(_currentStage);
        _updateGrowthDisposable = ReactiveExtensions.RepeatableDelayedCall(_ctx.asset.GrowthTime / _ctx.asset.StageCount, UpdateGrowth);
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