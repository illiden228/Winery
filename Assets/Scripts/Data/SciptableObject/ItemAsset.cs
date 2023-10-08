using UnityEngine;

public class ItemAsset : ScriptableObject
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _id;
    [SerializeField] protected string _description;
    [SerializeField] protected Sprite _icon;

    public string Name => _name;
    public string Id => _id;
    public string Description => _description;
    public Sprite Icon => _icon;
}
