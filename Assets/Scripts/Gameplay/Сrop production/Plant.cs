using Core;
using Game.Selectables;
using UnityEngine;
using UnityEngine.Rendering;

public class Plant : BaseMonobehaviour, IGrowable
{
    [SerializeField] private PlantAsset _plantAsset;    
    [SerializeField] private ParticleSystem _grownEffect;

    private float _currentGrowthTime;
    private GameObject[] _growthStages;

    public bool Grown => _currentGrowthTime >= _plantAsset.GrowthTime;

    protected override void Awake()
    {
        var prefabs = _plantAsset.GetGrowthStagePrefabs();

        _growthStages = new GameObject[prefabs.Length];

        for (int i = 0; i < prefabs.Length; i++)
        {
            _growthStages[i] = Instantiate(prefabs[i]);
            _growthStages[i].SetActive(false);
        }
    }

    public void UpdateGrowth(float deltaTime)
    {
        if (Grown)
            return;

        _currentGrowthTime += deltaTime;
        UpdatePlantView();
    }

    private void UpdatePlantView()
    {
        //to do: refactoring
        //
        int index = (int)Mathf.Lerp(0, _growthStages.Length - 1, Mathf.InverseLerp(0f, _plantAsset.GrowthTime, _currentGrowthTime));
        //

        for (int i = 0; i < _growthStages.Length; i++)
        {
            if (index == i)
                _growthStages[i].SetActive(true);
            else
                _growthStages[i].SetActive(false);
        }

        if (Grown && _grownEffect)
            _grownEffect.Play();
    }
}
