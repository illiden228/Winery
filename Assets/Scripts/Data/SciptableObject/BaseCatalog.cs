using NaughtyAttributes;
using UnityEngine;
using System.Linq;
using System.Collections;

public class BaseCatalog : ScriptableObject
{
    [ValidateInput("ValidateIds")]
    [SerializeField] protected ItemAsset[] _itemAssets;

    private bool ValidateIds(ItemAsset[] plantAssets)
    {
        if (_itemAssets == null)
            return true;

        for (int i = 0; i < plantAssets.Length; i++)
        {
            if (plantAssets[i] != null && string.IsNullOrEmpty(plantAssets[i].Id))
            {
                Debug.LogWarning("������ ID � �������� ��������!");
                return false;
            }
        }

        IEnumerable duplicateIds = plantAssets
        .GroupBy(Id => Id)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key);

        bool valid = ValidateType();

        if (!valid)
        {
            Debug.LogWarning("�� ������������ ���� �������� � ��������!");
            return false;
        }

        foreach (var id in duplicateIds)
        {
            Debug.LogWarning($"��������� id: {id}!");
            valid = false;
        }

        return valid;
    }

    public T GetAssetById<T>(string id) where T : ItemAsset
    {
        if (_itemAssets.GetType().GetElementType() is T)
        {
            for (int i = 0; i < _itemAssets.Length; i++)
            {
                if (_itemAssets[i].Id == id)
                    return _itemAssets[i] as T;
            }
        }
        return null;
    }

    protected virtual bool ValidateType()
    {
        return true;
    }
}
