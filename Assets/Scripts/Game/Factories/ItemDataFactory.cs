using Core;
using Data;
using Game.Factories;
using UnityEngine;

namespace Factories
{
    public class ItemDataFactory : BaseDisposable, IFactory<Item, ItemAsset>
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

        public Item CreateObject(ItemAsset asset)
        {
            Item item = null;
            switch (asset)
            {
                case SeedlingAsset:
                    {
                        SeedlingAsset seedlingAsset = _ctx.plantCatalog.GetAssetById<SeedlingAsset>(asset.Id);
                        item = new SeedlingData
                        {
                            Id = seedlingAsset.Id,
                            Name = seedlingAsset.Name,
                            MaxCount = seedlingAsset.MaxStackCount,
                            FruitRipeTime = seedlingAsset.FruitRipeTime,
                            GrowthTime = seedlingAsset.GrowthTime,
                            RipeStageCount = seedlingAsset.RipeStageCount,
                            SproutStageCount = seedlingAsset.SproutStageCount,
                            Icon = seedlingAsset.Icon,
                            Cost = seedlingAsset.Cost,
                            Production = seedlingAsset.Production,
                            ViewPrefabName = seedlingAsset.View.name,
                            ProductionCount = seedlingAsset.ProductionCount
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
                            ProductionTime = grapeAsset.ProductionTime,
                            Production = grapeAsset.Production,
                            ProductionCount = grapeAsset.ProductionCount
                        };
                        break;
                    }
                case JuiceAsset:
                    {
                        JuiceAsset juiceAsset = _ctx.juiceCatalog.GetAssetById<JuiceAsset>(asset.Id);
                        item = new JuiceData
                        {
                            Id = juiceAsset.Id,
                            Name = juiceAsset.Name,
                            Icon = juiceAsset.Icon,
                            ProductionTime = juiceAsset.ProductionTime,
                            Production = juiceAsset.Production,
                            ProductionCount = juiceAsset.ProductionCount
                        };
                        break;
                    }
                case WineAsset:
                    {
                        WineAsset wineAsset = _ctx.wineCatalog.GetAssetById<WineAsset>(asset.Id);
                        item = new WineData
                        {
                            Id = wineAsset.Id,
                            Name = wineAsset.Name,
                            Icon = wineAsset.Icon,    
                            ProductionCount = wineAsset.ProductionCount,
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