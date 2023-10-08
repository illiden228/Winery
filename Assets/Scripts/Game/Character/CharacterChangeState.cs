using System;
using Core;
using Data;
using Game.Selectables;
using Tools.Extensions;
using UI.Select;
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
            public ReactiveEvent<string> animationEvent;
            public ReactiveEvent<SelectorInfo> selectorEvent;
        }

        private readonly Ctx _ctx;
        private IDisposable _selectWaiting;

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
                    ReactiveProperty<Item> item = new ReactiveProperty<Item>();
                    
                    _selectWaiting?.Dispose();

                    _selectWaiting = item.SkipLatestValueOnSubscribe().Subscribe(item =>
                    {
                        _ctx.selectable.Value.Select(item);
                        _ctx.animationEvent.Notify(CharacterAnimation.Triggers.Take);
                        _ctx.selectable.Value = null;
                    });
                        
                    _ctx.selectorEvent.Notify(new SelectorInfo
                    {
                        Item = item,
                        Open = true
                    });
                }
                else
                {
                    _selectWaiting?.Dispose();
                    _ctx.selectable.Value = null;
                    _ctx.selectorEvent.Notify(new SelectorInfo
                    {
                        Open = false
                    });
                }
            }));
            
            
        }

        protected override void OnDispose()
        {
            _selectWaiting?.Dispose();
            base.OnDispose();
        }
    }
}