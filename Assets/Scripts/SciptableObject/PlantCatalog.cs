using NaughtyAttributes;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;

[CreateAssetMenu(fileName = "PlantCatalog", menuName = "ScriptableObjects/PlantCatalog", order = 0)]
public class PlantCatalog : ScriptableObject
{
    [ValidateInput("ValidateIds")]
    [SerializeField] private PlantAsset[] _plantAssets;

    private bool ValidateIds(PlantAsset[] plantAssets)
    {
        if (_plantAssets == null)
            return true;

        for (int i = 0; i < plantAssets.Length; i++)
        {
            if (plantAssets[i] != null && string.IsNullOrEmpty(plantAssets[i].Id))
            {
                Debug.LogWarning("Пустой ID в каталоге растений!");
                return false;
            }
        }

        IEnumerable duplicateIds = plantAssets
        .GroupBy(Id => Id)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key);

        bool valid = true;

        foreach (var id in duplicateIds)
        {
            Debug.LogWarning($"Дубликаты id: {id}!");
            valid = false;
        }

        return valid;
    }

    public PlantAsset GetPlantAssetById(string id)
    {
        for (int i = 0; i < _plantAssets.Length; i++)
        {
            if (_plantAssets[i].Id == id)
                return _plantAssets[i];
        }

        return null;
    }

    public PlantAsset[] GetCatalog()
    {
        return _plantAssets.ToArray();
    }
}
