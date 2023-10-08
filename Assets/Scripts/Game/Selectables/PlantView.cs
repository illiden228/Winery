using Core;
using System;
using UnityEngine;

public class PlantView : BaseMonobehaviour
{
    [SerializeField] private ParticleSystem _grownEffect;
    [SerializeField] private GameObject[] _growthStages;

    public struct Ctx
    {
        public float growthTime;
    }

    private Ctx _cxt;

    public void Init(Ctx ctx)
    {
        _cxt = ctx;

        for (int i = 0; i < _growthStages.Length; i++)
            _growthStages[i].SetActive(false);        
    }

    public void UpdatePlantView(int newState, bool plantGrown = false)
    {
        for (int i = 0; i < _growthStages.Length; i++)
        {
            if (newState - 1 == i)
                _growthStages[i].SetActive(true);
            else
                _growthStages[i].SetActive(false);            
        }

        if (plantGrown )
        {
            Debug.Log("Росток вырос!");

            if (_grownEffect)
                _grownEffect.Play();
        }
            
    }

    public void DestroyView()
    {
       Destroy(gameObject);
    }
}
