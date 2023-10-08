using UnityEngine;

[CreateAssetMenu(fileName = "PlantAsset", menuName = "ScriptableObjects/PlantAsset", order = 0)]
public class PlantAsset : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _id;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private float _growTime;
    [SerializeField] private int _stages = 4;
    [SerializeField] private PlantView _view;
    //[SerializeField] private GameObject[] _growthStagePrefabs;
    [SerializeField] private int _cost;

    public string Name => _name;
    public string Id => _id;
    public string Description => _description;
    public Sprite Icon => _icon;
    public float GrowthTime => _growTime;
	public int Cost => _cost;
	public PlantView View => _view;
    public int StageCount => _stages;

    //public GameObject[] GetGrowthStagePrefabs()
    //{
    //    return _growthStagePrefabs;
    //}
}
