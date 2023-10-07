using Core;
using UnityEngine;

namespace Game.Selectables
{
    public class SoilPm : BaseDisposable
    {
        public struct Ctx
        {
            public SoilView view;
            public int id;
        }
        
        public enum SoilState
        {
            Empty,
            Grows,
            GrownUp
        }

        public enum SoilAction
        {
            PlaceGrowable,
            WaterGrowable,
            Harvest
        }

        private readonly Ctx _ctx;
        private Plant _currentPlant;

        public SoilPm(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.view.Init(new SoilView.Ctx
            {
                onSelect = OnSelect
            });
        }

        private void OnSelect()
        {
            Debug.Log($"Выьрана грядка {_ctx.id}");
        }
    }
}