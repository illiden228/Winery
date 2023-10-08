using Core;
using Game.Selectables;
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
            _targetPosition = AddDispose(new ReactiveProperty<Vector3>());
            _ctx.resourceLoader.LoadPrefab("fake", CHARACTER_VIEW_PREFAB_NAME, OnViewLoaded);
        }

        private void OnViewLoaded(GameObject view)
        {
            _view = GameObject.Instantiate(view, _ctx.startPosition, Quaternion.identity).GetComponent<CharacterView>();

            ReactiveProperty<ISelectable> selectable = AddDispose(new ReactiveProperty<ISelectable>());
            ReactiveProperty<Vector3> newPosition = AddDispose(new ReactiveProperty<Vector3>());
            CharacterModel characterModel = new CharacterModel
            {
                Speed = new ReactiveProperty<float>(3f)
            };
            CharacterMovePm.Ctx characterMoveCtx = new CharacterMovePm.Ctx
            {
                model = characterModel,
                view = _view,
                targetPosition = _targetPosition,
            };
            AddDispose(new CharacterMovePm(characterMoveCtx));

            CharacterTargeter.Ctx targeterCtx = new CharacterTargeter.Ctx
            {
                targetPosition = newPosition,
                selectable = selectable,
                camera = _ctx.camera,
                
            };
            AddDispose(new CharacterTargeter(targeterCtx));

            CharacterChangeState.Ctx changeStateCtx = new CharacterChangeState.Ctx
            {
                selectable = selectable,
                newPosition = newPosition,
                targetPosition = _targetPosition,
                model = characterModel
            };
            AddDispose(new CharacterChangeState(changeStateCtx));
        }
    }
}