using UnityEngine;

[CreateAssetMenu(fileName = "PlantAsset", menuName = "ScriptableObjects/ItemAssets/PlantAsset", order = 0)]
public class PlantAsset : ItemAsset
{    
    [SerializeField] private float _growTime;
    [SerializeField] private float _fruitRipeTime;
    [SerializeField] private int _sproutStages = 4;
    [SerializeField] private int _ripeStages = 2;
    [SerializeField] private PlantView _view;
    //[SerializeField] private GameObject[] _growthStagePrefabs;
    [SerializeField] private int _cost;
    [SerializeField] private int _maxStackCount;
        
    public float GrowthTime => _growTime;
    public float FruitRipeTime => _fruitRipeTime;
    public int Cost => _cost;
	public PlantView View => _view;    
    public int SproutStageCount => _sproutStages;
    public int RipeStageCount => _ripeStages;
    public int MaxStackCount => _maxStackCount;

    //public GameObject[] GetGrowthStagePrefabs()
    //{
    //    return _growthStagePrefabs;
    //}
}
