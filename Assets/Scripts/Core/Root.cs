using Core;
using Game.Character;
using Tools;
using Unity.VisualScripting;
using UnityEngine;

public class Root : BaseMonobehaviour
{
    [SerializeField] private Transform _startPositon;
    [SerializeField] private Camera _camera;
    private IResourceLoader _resourceLoader;
    private CharacterPm _character;
    
    private void Awake()
    {
        _resourceLoader = new ResourcePreLoader(new ResourcePreLoader.Ctx
        {
            maxLoadDelay = 0f,
            minLoadDelay = 0f
        });

        CharacterPm.Ctx characterCtx = new CharacterPm.Ctx
        {
            resourceLoader = _resourceLoader,
            startPosition = _startPositon.position,
            camera = _camera
        };
        _character = new CharacterPm(characterCtx);
    }

    protected override void OnDestroy()
    {
        _resourceLoader.Dispose();
        _character.Dispose();
        base.OnDestroy();
    }
}
