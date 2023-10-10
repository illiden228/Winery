﻿using Cinemachine;
using Core;
using Data;
using Game.Selectables;
using Tools;
using Tools.Extensions;
using UI.Select;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Character
{
    public class CharacterPm : BaseDisposable
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public Vector3 startPosition;
            public Camera camera;
            public float startSpeed;
            public ReactiveEvent<SelectorInfo> selectorEvent;
            public EventSystem eventSystem;
            public CinemachineVirtualCamera followCamera;
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
            CharacterModel characterModel = new CharacterModel
            {
                Speed = new ReactiveProperty<float>(_ctx.startSpeed)
            };
            
            ReactiveProperty<ISelectable> selectable = AddDispose(new ReactiveProperty<ISelectable>());
            ReactiveProperty<Vector3> newPosition = AddDispose(new ReactiveProperty<Vector3>());
            ReactiveEvent<string> animationEvent = new ReactiveEvent<string>();
            _view = GameObject.Instantiate(view, _ctx.startPosition, Quaternion.identity).GetComponent<CharacterView>();
            _view.Init(new CharacterView.Ctx
            {
                viewDisposable = AddDispose(new CompositeDisposable()),
                animationAction = animationEvent,
                isMove = characterModel.IsMove,
                followCamera = _ctx.followCamera
            });

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
                eventSystem = _ctx.eventSystem
            };
            AddDispose(new CharacterTargeter(targeterCtx));

            CharacterChangeState.Ctx changeStateCtx = new CharacterChangeState.Ctx
            {
                selectable = selectable,
                newPosition = newPosition,
                targetPosition = _targetPosition,
                model = characterModel,
                animationEvent = animationEvent,
                selectorEvent = _ctx.selectorEvent
            };
            AddDispose(new CharacterChangeState(changeStateCtx));
        }
    }
}