using System.Collections.Generic;
using Core;
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
        
        [SerializeField] private Transform _startPosition;
        [SerializeField] private Camera _camera;
        [SerializeField] private Canvas _mainCanvas;    
        [SerializeField] private EventSystem _eventSystem;

        public List<SoilView> Soils => _soils;

        public List<JuicerView> Juicers => _juicers;

        public Transform StartPosition => _startPosition;

        public Camera Camera => _camera;

        public Canvas MainCanvas => _mainCanvas;

        public EventSystem EventSystem => _eventSystem;
    }
}