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
        private ReactiveProperty<SelectableStatus> _currentStatus;

        public CharacterChangeState(Ctx ctx)
        {
            _ctx = ctx;
            _currentStatus = new ReactiveProperty<SelectableStatus>();

            AddDispose(_ctx.newPosition.Subscribe(pos =>
            {
                _ctx.targetPosition.Value = pos;
            }));

            AddDispose(_ctx.model.IsMove.Subscribe(isMove =>
            {
                if (isMove)
                {
                    if(_ctx.selectable.Value == null)
                        return;
                    if(_currentStatus.Value == null || !_currentStatus.Value.NeedSelector)
                        return;
                    _selectWaiting?.Dispose();
                    _ctx.selectable.Value = null;
                    _ctx.selectorEvent.Notify(new SelectorInfo
                    {
                        Open = false
                    });
                }
                else if (_ctx.selectable.Value != null)
                {
                    if(_currentStatus.Value == null)
                        return;
                    
                    if (!_currentStatus.Value.NeedSelector)
                    {
                        Select(null, _currentStatus.Value.AnimationTriggerName);
                        return;
                    }
                    
                    _selectWaiting?.Dispose();
                        
                    ReactiveProperty<Item> item = new ReactiveProperty<Item>();

                    _selectWaiting = item.SkipLatestValueOnSubscribe().Subscribe(item =>
                    {
                        Select(item, _currentStatus.Value.AnimationTriggerName);
                    });
                        
                    _ctx.selectorEvent.Notify(new SelectorInfo
                    {
                        Item = item,
                        Open = true
                    });
                }
            }));

            AddDispose(_ctx.selectable.Subscribe(selectable =>
            {
                if (selectable == null)
                {
                    _currentStatus.Value = null;
                    return;
                }

                _currentStatus.Value = selectable.GetSelectState();
            }));

            AddDispose(_currentStatus.Subscribe(status =>
            {
                if (status == null)
                    _ctx.selectable.Value = null;
            }));
        }

        private void Select(Item item, string animation)
        {
            _ctx.selectable.Value.Activate(item);
            _ctx.animationEvent.Notify(animation);
            _ctx.selectable.Value = null;
        }

        protected override void OnDispose()
        {
            _selectWaiting?.Dispose();
            base.OnDispose();
        }
    }
}