using Core;
using Data;
using UnityEngine;

namespace Game.Factories
{
    public class PlantFactory : BaseDisposable
    {
        public struct Ctx
        {
            public PlantCatalog plantCatalog;
        }
        
        private readonly Ctx _ctx;
        private const string PlantViewName = "PlantView";

        public PlantFactory(Ctx ctx)
        {
            _ctx = ctx;
        }

        public PlantPm GetPlantPmCtxById(SeedlingData item, Transform parent)
        {
            PlantView plantView = GameObject.Instantiate(_ctx.plantCatalog.GetAssetById<PlantAsset>("IsabellaGrape").View);
            plantView.transform.SetParent(parent, false);

            return new PlantPm(new PlantPm.Ctx
            {
                plantView = plantView,
                seedling = item,                
            });
        }
    }
}