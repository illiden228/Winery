using System.Collections.Generic;
using Core;
using Game.Selectables;
using Tools.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Character
{
    public class CharacterTargeter : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveProperty<ISelectable> selectable;
            public ReactiveProperty<Vector3> targetPosition;
            public Camera camera;
            public EventSystem eventSystem;
        }

        private readonly Ctx _ctx;

        public CharacterTargeter(Ctx ctx)
        {
            _ctx = ctx;
            ReactiveExtensions.StartUpdate(CheckGroundAnd);
        }

        private void CheckGroundAnd()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(IsPointerOverUIObject())
                    return;
                Ray ray = _ctx.camera.ScreenPointToRay(Input.mousePosition);
                RaycastGround(ray);
                RaycastSelectables(ray);
            }
        }

        private void RaycastGround(Ray ray)
        {
            if(Physics.Raycast(ray, out var hit, Mathf.Infinity, Layers.GroundMask))
                _ctx.targetPosition.Value = hit.point;
        }
        
        private void RaycastSelectables(Ray ray)
        {
            ISelectable selectable = null;

            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, Layers.SelectablesMask);
            foreach (var raycastHit in hits)
            {
                if (selectable == null)
                {
                    selectable = raycastHit.collider.GetComponent<ISelectable>();
                    _ctx.selectable.SetValueAndForceNotify(selectable);
                    break;
                }
            }
        }
        
        private bool IsPointerOverUIObject()
        {
#if UNITY_EDITOR
            return _ctx.eventSystem.IsPointerOverGameObject();
#else
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if(_ctx.eventSystem.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return true;
                }
            }
#endif
            return false;
        }
    }
}