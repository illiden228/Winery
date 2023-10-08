using UnityEngine;

[CreateAssetMenu(fileName = "JuiceAsset", menuName = "ScriptableObjects/ItemAssets/JuiceAsset", order = 0)]
public class JuiceAsset : ItemAsset
{
    [SerializeField] private ItemAsset _production;
    [SerializeField] private float _productionTime;

    public ItemAsset Production => _production;
    public float ProductionTime => _productionTime;
}
