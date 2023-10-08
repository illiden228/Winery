using Core;
using Data;
using Tools.Extensions;
using UniRx;
using UnityEngine;

namespace Factories
{
    public class ItemDataFactory : BaseDisposable
    {
        public struct Ctx
        {
            public PlantCatalog plantCatalog;
        }

        private readonly Ctx _ctx;

        public ItemDataFactory(Ctx ctx)
        {
            _ctx = ctx;
        }

        public Item CreateObject(ItemAsset asset, int count)
        {
            Item item = null;
            switch (asset)
            {
                case PlantAsset:
                    PlantAsset plantAsset = _ctx.plantCatalog.GetAssetById<PlantAsset>(asset.Id);
                    item = new SeedlingData
                    {
                        Id = plantAsset.Id,
                        Name = plantAsset.Name,
                        MaxCount = plantAsset.MaxStackCount,
                        FruitRipeTime = plantAsset.FruitRipeTime,
                        GrowthTime = plantAsset.GrowthTime,
                        RipeStageCount = plantAsset.RipeStageCount,
                        SproutStageCount = plantAsset.SproutStageCount,
                        Icon = plantAsset.Icon,
                        Cost = plantAsset.Cost,
                        Count = count
                    };
                    break;
                default:
                    Debug.Log($"Don't find asset with id {asset.Id}");
                    break;
            }

            return item;
        }
    }
}