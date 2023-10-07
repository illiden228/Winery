using Core;
using NaughtyAttributes;
using System;
using UnityEngine;

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
        
    public void PlaceGrowable(Plant plantPrefab, Action<bool> callBack)
    {
        if (_currentSoilState != SoilState.Empty)
        {
            callBack?.Invoke(false);
            Debug.Log("Soil is slot occupied!");
        }

        //temp
        _plantPrefab = plantPrefab;
        //

        //to do: replace with pool
        _growable = Instantiate(_plantPrefab, transform);

        callBack?.Invoke(true);
    }
}
