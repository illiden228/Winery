using System;
using Core;
using Game.Selectables;
using TMPro;
using Tools.Extensions;
using UniRx;
using UnityEngine;

namespace Game.Character
{
    public class CharacterMovePm : BaseDisposable
    {
        public struct Ctx
        {
            public IReadOnlyReactiveProperty<Vector3> targetPosition;
            public CharacterView view;
            public CharacterModel model;
            public IReadOnlyReactiveProperty<ISelectable> selectable;
        }

        private readonly Ctx _ctx;
        private IDisposable _moveDisposable;
        private float _offset;

        public CharacterMovePm(Ctx ctx)
        {
            _ctx = ctx;

            AddDispose(_ctx.targetPosition.Subscribe(position =>
            {
                if (_moveDisposable != null)
                    _moveDisposable.Dispose();
                _ctx.model.IsMove.Value = true;
                SoundManager.Instance.ToggleSteps(true);
                
                _moveDisposable = ReactiveExtensions.StartUpdate(() =>
                {
                    if (!TryMoveToPosition(position, _offset))
                    {
                        _moveDisposable.Dispose();
                        _ctx.model.IsMove.Value = false;
                        SoundManager.Instance.ToggleSteps(false);
                    }
                });
            }));

            AddDispose(_ctx.selectable.Subscribe(selectable =>
            {
                if (selectable == null)
                {
                    _offset = 0;
                    return;
                }

                _offset = selectable.Offset;
            }));
        }

        private bool TryMoveToPosition(Vector3 position, float offset)
        {
            Vector3 distance = position - _ctx.view.transform.position;
            bool canMove = distance.sqrMagnitude > 0.001f + offset * offset;
            _ctx.view.transform.forward = distance;
            if (canMove)
            {
                _ctx.view.transform.position = Vector3.MoveTowards(_ctx.view.transform.position, position, _ctx.model.Speed.Value * Time.deltaTime);
            }
            return canMove;
        }

        protected override void OnDispose()
        {
            _moveDisposable.Dispose();
            base.OnDispose();
        }
    }
}