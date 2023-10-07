using Core;
using NaughtyAttributes;
using System;
using Game.Character;
using Game.Selectables;
using UnityEngine;

public class SoilView : BaseMonobehaviour, ISelectable
{
    public struct Ctx
    {
        public Action onSelect;
    }

    private Ctx _cxt;

    public void Init(Ctx ctx)
    {
        _cxt = ctx;
    }
    
    public void Select()
    {
        _cxt.onSelect?.Invoke();
    }
    
    

    // [SerializeField] private float _timeToSlowUpdate = 0.5f;
    // [HorizontalLine(color: EColor.Green)]
    // [Header("Test")]
    // [SerializeField] private Plant _plantPrefab;
    //
    // private float _updateTimer;
    // private IGrowable _growable;
    // private SoilState _currentSoilState;
    //
    // public SoilState State => _currentSoilState;
    //
    // [Button("Place plant")]
    // private void PlacePlant()
    // {
    //     _growable = Instantiate(_plantPrefab, transform);
    // }
    //
    // private void Update()
    // {
    //     _updateTimer += Time.deltaTime;
    //     if (_updateTimer >= _timeToSlowUpdate)
    //     {
    //         SlowUpdate(_updateTimer);
    //         _updateTimer = 0f;
    //     }
    // }
    //
    // private void SlowUpdate(float deltaTime)
    // {
    //     if (_growable != null)
    //         _growable.UpdateGrowth(deltaTime);
    // }
    //     
    // public void PlaceGrowable(Plant plantPrefab, Action<bool> callBack)
    // {
    //     if (_currentSoilState != SoilState.Empty)
    //     {
    //         callBack?.Invoke(false);
    //         Debug.Log("Soil is slot occupied!");
    //     }
    //
    //     //temp
    //     _plantPrefab = plantPrefab;
    //     //
    //
    //     //to do: replace with pool
    //     GameObject growableGO = Instantiate(_plantPrefab.gameObject, transform);
    //     growableGO.transform.localPosition = Vector3.zero;
    //
    //     _growable = growableGO.GetComponent<IGrowable>();
    //
    //     callBack?.Invoke(true);
    // }

    // public void Select()
    // {
    //     PlaceGrowable(_plantPrefab, null);
    // }
}
