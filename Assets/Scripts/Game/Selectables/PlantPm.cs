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
    private int _currentStep;
    private IDisposable _updateGrowthDisposable;
    public ReactiveEvent _waitForCollect;
    private PlantStages _currentPlantStage;

    public bool Grown => _currentStep == _ctx.asset.SproutStageCount;    
    public bool Ripened => _currentStep == _ctx.asset.RipeStageCount;

    public PlantPm(Ctx ctx)
    {
        _ctx = ctx;
        _ctx.plantView.Init(new PlantView.Ctx { growthTime = _ctx.asset.GrowthTime, fruitRipeTime = _ctx.asset.FruitRipeTime });
        _currentPlantStage = PlantStages.Growing;
        _currentStep = 1;
        _ctx.plantView.UpdatePlantView(_currentStep);
        _updateGrowthDisposable = ReactiveExtensions.RepeatableDelayedCall(_ctx.asset.GrowthTime / _ctx.asset.SproutStageCount, UpdateGrowth);
    }

    public void UpdateGrowth()
    {
        switch (_currentPlantStage)
        {
            case PlantStages.Growing:
                {
                    _currentStep++;
                    _ctx.plantView.UpdatePlantView(_currentStep, Grown);

                    if (Grown)
                    {
                        _currentStep = 1;
                        _currentPlantStage = PlantStages.FruitsRipening;

                        if (_updateGrowthDisposable != null)
                            _updateGrowthDisposable.Dispose();

                        _updateGrowthDisposable = ReactiveExtensions.RepeatableDelayedCall(_ctx.asset.FruitRipeTime / _ctx.asset.RipeStageCount, UpdateGrowth);
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

                    if (Ripened)
                    {
                        if (_updateGrowthDisposable != null)
                            _updateGrowthDisposable.Dispose();

                        _currentPlantStage = PlantStages.FruitsRipened;
                    }

                    break;
                }
            case PlantStages.FruitsRipened:
                {
                    _currentStep = 1;
                    _currentPlantStage = PlantStages.FruitsRipening;                                       

                    _updateGrowthDisposable = ReactiveExtensions.RepeatableDelayedCall(_ctx.asset.FruitRipeTime / _ctx.asset.RipeStageCount, UpdateGrowth);
                    break;
                }
        }
    }

    public void PickUpFruits(Action callBack)
    {
        if (_currentPlantStage != PlantStages.FruitsRipened)
            return;

        UpdateGrowth();

        callBack?.Invoke();
    }

    protected override void OnDispose()
    {
        if (_updateGrowthDisposable != null)
            _updateGrowthDisposable.Dispose();

        _ctx.plantView.DestroyView();

        base.OnDispose();
    }
}