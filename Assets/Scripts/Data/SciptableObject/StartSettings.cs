using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Start Settings", menuName = "Settings/Start", order = 0)]
    public class StartSettings : ScriptableObject
    {
        [SerializeField] private float _characterSpeed;
        [SerializeField] private int _startMoneys;
        [SerializeField] private List<SeedlingAsset> _startPlants;
        [SerializeField] private List<SeedlingAsset> _startStock;
        [SerializeField] private float _carTime;
        [BoxGroup("Scriptable objects")]
        [SerializeField] private PlantCatalog _plantCatalog;

        [BoxGroup("Scriptable objects")]
        [SerializeField] private GrapeCatalog _grapeCatalog;
        [BoxGroup("Scriptable objects")]
        [SerializeField] private JuiceCatalog _juiceCatalog;
        [BoxGroup("Scriptable objects")]
        [SerializeField] private WineCatalog _wineCatalog;

        public float CharacterSpeed => _characterSpeed;
        public int StartMoneys => _startMoneys;
        public List<SeedlingAsset> StartPlants => _startPlants;
        public List<SeedlingAsset> StartStock => _startStock;
        public PlantCatalog PlantCatalog => _plantCatalog;
        public GrapeCatalog GrapeCatalog => _grapeCatalog;
        public JuiceCatalog JuiceCatalog => _juiceCatalog;
        public WineCatalog WineCatalog => _wineCatalog;
        public float CarTime => _carTime;
    }
}
