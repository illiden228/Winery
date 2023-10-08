using Core;
using UnityEngine;

public class PlantView : BaseMonobehaviour
{
    [SerializeField] private ParticleSystem _grownEffect;
        
    private GameObject[] _growthStages;

    public struct Ctx
    {
        public float growthTime;
    }

    private Ctx _cxt;

    public void Init(Ctx ctx, GameObject[] growthStagesPrefabs)
    {
        _cxt = ctx;

        _growthStages = new GameObject[growthStagesPrefabs.Length];
        for (int i = 0; i < growthStagesPrefabs.Length; i++)
        {
            _growthStages[i] = Instantiate(growthStagesPrefabs[i], transform);
            _growthStages[i].transform.localPosition = new Vector3(0, _growthStages[i].transform.localPosition.y, 0);
            _growthStages[i].SetActive(false);
        }
    }
    
    public void UpdatePlantView(float currentGrowthTime, bool playEffect = false)
    {
        //to do: refactoring
        //
        int index = (int)Mathf.Lerp(0, _growthStages.Length - 1, Mathf.InverseLerp(0f, _cxt.growthTime, currentGrowthTime));
        //

        for (int i = 0; i < _growthStages.Length; i++)
        {
            if (index == i)
                _growthStages[i].SetActive(true);
            else
                _growthStages[i].SetActive(false);
        }

        if (playEffect && _grownEffect)
            _grownEffect.Play();
    }
}
