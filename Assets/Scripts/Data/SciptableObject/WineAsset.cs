using UnityEngine;

[CreateAssetMenu(fileName = "WineAsset", menuName = "ScriptableObjects/ItemAssets/WineAsset", order = 0)]
public class WineAsset : ItemAsset
{
    [SerializeField] private int _productionCount;

    public int ProductionCount => _productionCount;
}
