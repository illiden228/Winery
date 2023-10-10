using System.Collections.Generic;
using Cinemachine;
using Core;
using Game.Selectables;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools
{
    public class SceneContext : BaseMonobehaviour
    {
        [BoxGroup("Scene objects")]
        [SerializeField] private List<SoilView> _soils;
        [BoxGroup("Scene objects")]
        [SerializeField] private List<JuicerView> _juicers;
        [BoxGroup("Scene objects")]
        [SerializeField] private List<BarrelView> _barrels;
        [BoxGroup("Scene objects")]
        [SerializeField] private CarView _car;
        
        [SerializeField] private Transform _startPosition;
        [SerializeField] private Camera _camera;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Canvas _mainCanvas;    
        [SerializeField] private EventSystem _eventSystem;

        public List<SoilView> Soils => _soils;

        public List<JuicerView> Juicers => _juicers;
        
        public List<BarrelView> Barrels => _barrels;

        public CarView Car => _car;

        public Transform StartPosition => _startPosition;

        public Camera Camera => _camera;

        public Canvas MainCanvas => _mainCanvas;

        public EventSystem EventSystem => _eventSystem;

        public CinemachineVirtualCamera VirtualCamera => _virtualCamera;
    }
}