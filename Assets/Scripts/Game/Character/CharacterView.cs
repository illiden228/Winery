using Core;
using Tools.Extensions;
using UniRx;
using UnityEngine;

namespace Game.Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterView : BaseMonobehaviour
    {
        public struct Ctx
        {
            public CompositeDisposable viewDisposable;
            public IReadOnlyReactiveEvent<string> animationAction;
            public IReadOnlyReactiveProperty<bool> isMove;
        }

        private Ctx _ctx;
        private Animator _animator;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            _animator = GetComponent<Animator>();
            _ctx.animationAction.SubscribeWithSkip(_animator.SetTrigger).AddTo(_ctx.viewDisposable);
            _ctx.isMove.Subscribe(move => _animator.SetBool(CharacterAnimation.Bool.Move, move))
                .AddTo(_ctx.viewDisposable);
        }
    }
}