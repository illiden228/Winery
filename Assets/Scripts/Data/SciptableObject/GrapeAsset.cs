using UnityEngine;

[CreateAssetMenu(fileName = "GrapeAsset", menuName = "ScriptableObjects/ItemAssets/GrapeAsset", order = 0)]
public class GrapeAsset : ItemAsset
{
    [SerializeField] private ItemAsset _production;
    [SerializeField] private float _productionTime;
    [SerializeField] private int _productionCount;

    public ItemAsset Production => _production;
    public float ProductionTime => _productionTime;
    public int ProductionCount => _productionCount;
}
