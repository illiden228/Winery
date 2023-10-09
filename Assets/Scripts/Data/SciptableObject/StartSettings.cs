using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Start Settings", menuName = "Settings/Start", order = 0)]
    public class StartSettings : ScriptableObject
    {
        [SerializeField] private float _characterSpeed;
        [SerializeField] private int _startMoneys;
        [SerializeField] private List<PlantAsset> _startPlants;
        [SerializeField] private List<PlantAsset> _startStock;

        public float CharacterSpeed => _characterSpeed;
        public int StartMoneys => _startMoneys;

        public List<PlantAsset> StartPlants => _startPlants;
        public List<PlantAsset> StartStock => _startStock;
    }
}