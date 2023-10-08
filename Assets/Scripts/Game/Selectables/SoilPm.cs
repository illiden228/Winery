using Core;
using UnityEngine;
using Game.Factories;
using Game.Player;

namespace Game.Selectables
{
    public class SoilPm : BaseDisposable
    {
        public struct Ctx
        {
            public SoilView view;
            public PlantFactory plantFactory;
            public Inventory inventory;
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
        private PlantPm _currentPm;

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
            if (_currentPm == null)
            {
                _currentPm = (_ctx.plantFactory.GetPlantPmCtxById("IsabellaGrape", _ctx.view.transform));
                Debug.Log($"Выбрана грядка {_ctx.id} и посажен росток IsabellaGrape!");
            }
            else
            {
                if (_currentPm.Ripened)
                    _currentPm.PickUpFruits(() =>
                    {
                        //_ctx.inventory.AddItemToInventory();
                    });
            }
        }
    }
}