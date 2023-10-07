using Core;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Soil : BaseMonobehaviour
{
    public enum SoilState
    {
        Empty,
        Grows,                
        GrownUp
    }

    public enum SoilAction
    {
        PlaceGrowable,
        WaterGrowable,
        Harvest
    }

    [SerializeField] private float _timeToSlowUpdate = 0.5f;
    [HorizontalLine(color: EColor.Green)]
    [Header("Test")]
    [SerializeField] private Plant _plantPrefab;

    private float _updateTimer;
    private IGrowable _growable;
    private SoilState _currentSoilState;

    public SoilState State => _currentSoilState;

    [Button("Place plant")]
    private void PlacePlant()
    {
        _growable = Instantiate(_plantPrefab, transform);
    }

    private void Update()
    {
        _updateTimer += Time.deltaTime;
        if (_updateTimer >= _timeToSlowUpdate)
        {
            SlowUpdate(_updateTimer);
            _updateTimer = 0f;
        }
    }

    private void SlowUpdate(float deltaTime)
    {
        if (_growable != null)
            _growable.UpdateGrowth(deltaTime);
    }

    public void CallAction(SoilAction soilAction, Action<int> amountCallBack)
    {
        switch(soilAction)
        {
            case SoilAction.PlaceGrowable:
                {
                    break;
                }
            case SoilAction.WaterGrowable:
                {
                    break;
                }
            case SoilAction.Harvest:
                {
                    if (_growable.Grown)
                        amountCallBack?.Invoke(1);
                    break;
                }
        }
    }
}
