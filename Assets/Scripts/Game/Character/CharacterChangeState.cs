using Core;
using Game.Selectables;
using UniRx;
using UnityEngine;

namespace Game.Character
{
    public class CharacterChangeState : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveProperty<ISelectable> selectable;
            public ReactiveProperty<Vector3> targetPosition;
            public ReactiveProperty<Vector3> newPosition;
            public CharacterModel model;
        }

        private readonly Ctx _ctx;

        public CharacterChangeState(Ctx ctx)
        {
            _ctx = ctx;

            AddDispose(_ctx.newPosition.Subscribe(pos =>
            {
                _ctx.targetPosition.Value = pos;
            }));

            AddDispose(_ctx.model.IsMove.Subscribe(isMove =>
            {
                if (!isMove && _ctx.selectable.Value != null)
                {
                    _ctx.selectable.Value.Select();
                    _ctx.selectable.Value = null;
                }
            }));
        }
    }
}