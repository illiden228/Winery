using Core;
using Data;
using Tools.Extensions;
using UnityEngine;

namespace Factories
{
    public class ItemDataFactory : BaseDisposable
    {
        public struct Ctx
        {
            public PlantCatalog plantCatalog;
            public GrapeCatalog grapeCatalog;
            public JuiceCatalog juiceCatalog;
            public WineCatalog wineCatalog;
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
                    {
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
                            Count = count,
                            Production = plantAsset.Production
                        };
                        break;
                    }
                case GrapeAsset:
                    {
                        GrapeAsset grapeAsset = _ctx.grapeCatalog.GetAssetById<GrapeAsset>(asset.Id);
                        item = new GrapeData
                        {
                            Id = grapeAsset.Id,
                            Name = grapeAsset.Name,
                            Icon = grapeAsset.Icon,
                            Count = count
    };
                        break;
                    }
                case JuiceAsset:
                    {
                        JuiceAsset grapeAsset = _ctx.juiceCatalog.GetAssetById<JuiceAsset>(asset.Id);
                        item = new JuiceData
                        {
                            Id = grapeAsset.Id,
                            Name = grapeAsset.Name,
                            Icon = grapeAsset.Icon,
                            Count = count
                        };
                        break;
                    }
                case WineAsset:
                    {
                        WineAsset grapeAsset = _ctx.wineCatalog.GetAssetById<WineAsset>(asset.Id);
                        item = new WineData
                        {
                            Id = grapeAsset.Id,
                            Name = grapeAsset.Name,
                            Icon = grapeAsset.Icon,
                            Count = count
                        };
                        break;
                    }
                default:
                    Debug.Log($"Don't find asset with id {asset.Id}");
                    break;
            }

            return item;
        }
    }
}