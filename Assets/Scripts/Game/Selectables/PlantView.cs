using Core;

using System;
using UnityEngine;

public class PlantView : BaseMonobehaviour
{
    [SerializeField] private ParticleSystem _grownEffect;
    [SerializeField] private ParticleSystem _fruitRipened;
    [SerializeField] private GameObject[] _growthSproutStages;
    [SerializeField] private GameObject[] _fruitRipeStages;

    public struct Ctx
    {
        public float growthTime;
        public float fruitRipeTime;
    }

    public enum PlantViewStage
    {
        Sprout,
        Fruitful
    }

    private Ctx _cxt;
    private PlantViewStage _currentPlantViewStage;

    public void Init(Ctx ctx)
    {
        _cxt = ctx;

        for (int i = 0; i < _growthSproutStages.Length; i++)
            _growthSproutStages[i].SetActive(false);

        _currentPlantViewStage = PlantViewStage.Sprout;
    }

    public void UpdatePlantView(int newState, bool stageFinished = false)
    {
        switch (_currentPlantViewStage)
        {
            case PlantViewStage.Sprout:
                {
                    UpdatePlantModels(_growthSproutStages, newState);

                    if (stageFinished)
                    {
                        Debug.Log("Росток вырос!");

                        if (_grownEffect)
                            _grownEffect.Play();
                    }
                    break;
                }
            case PlantViewStage.Fruitful:
                {
                    UpdatePlantModels(_fruitRipeStages, newState);

                    if (stageFinished)
                    {
                        Debug.Log("Плоды созрели!");

                        if (_fruitRipened)
                            _fruitRipened.Play();
                    }
                    break;
                }
        }
    }

    private void UpdatePlantModels(GameObject[] models, int newState)
    {
        if (models == null)
        {
            Debug.LogWarning("Пустой список моделей роста/созревания!");
            return;
        }

        for (int i = 0; i < models.Length; i++)
        {
            if (newState - 1 == i)
                models[i].SetActive(true);
            else
                models[i].SetActive(false);
        }
    }

    public void DestroyView()
    {
       Destroy(gameObject);
    }
}
