using Core;
using Game.Selectables;
using UnityEngine;

namespace Game.Factories
{
    public class PlantFactoryPm : BaseDisposable
    {
        public struct Ctx
        {
            public PlantCatalog plantCatalog;
            public FactoryView factoryView;
        }
        
        private readonly Ctx _ctx;
        private const string PlantViewName = "PlantView";

        public PlantFactoryPm(Ctx ctx)
        {
            _ctx = ctx;
        }

        public PlantPm GetPlantPmCtxById(string id, Transform parent)
        {
            PlantAsset plantAsset = _ctx.plantCatalog.GetPlantAssetById(id);           
            PlantView plantView = _ctx.factoryView.GetPrefabInstanceWithComponent<PlantView>(PlantViewName);
            plantView.transform.SetParent(parent, false);

            return new PlantPm(new PlantPm.Ctx
            {
                plantView = plantView,
                asset = plantAsset
            });
        }
    }
}