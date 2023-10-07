using Core;
using Tools.Extensions;
using UniRx;
using UnityEngine;

namespace Game.Character
{
    public class CharacterTargeter : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveProperty<Vector3> targetPosition;
            public Camera camera;
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
                if (Physics.Raycast(_ctx.camera.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity, Layers.GroundLayer))
                {
                    _ctx.targetPosition.Value = hit.point;
                }
            }
        }
    }
}