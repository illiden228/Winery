using Core;
using Tools;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace Game.Character
{
    public class CharacterPm : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public Vector3 startPosition;
            public Camera camera;
        }

        private readonly Ctx _ctx;
        private const string CHARACTER_VIEW_PREFAB_NAME = "Character";
        private CharacterView _view;
        private ReactiveProperty<Vector3> _targetPosition;

        public CharacterPm(Ctx ctx)
        {
            _ctx = ctx;
            _targetPosition = new ReactiveProperty<Vector3>();
            _ctx.resourceLoader.LoadPrefab("fake", CHARACTER_VIEW_PREFAB_NAME, OnViewLoaded);
        }

        private void OnViewLoaded(GameObject view)
        {
            _view = GameObject.Instantiate(view, _ctx.startPosition, Quaternion.identity).GetComponent<CharacterView>();
            CharacterMovePm.Ctx characterMoveCtx = new CharacterMovePm.Ctx
            {
                speed = 1f,
                view = _view,
                targetPosition = _targetPosition
            };
            AddDispose(new CharacterMovePm(characterMoveCtx));

            CharacterTargeter.Ctx targeterCtx = new CharacterTargeter.Ctx
            {
                targetPosition = _targetPosition,
                camera = _ctx.camera
            };
            AddDispose(new CharacterTargeter(targeterCtx));
        }
    }
}