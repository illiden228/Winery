using Core;
using Data;
using Factories;
using Game.Player;
using Game.Selectables;
using Tools;
using UnityEngine;

namespace Game.Factories
{
    public class ProductionGeneratorFactory : BaseDisposable, IFactory<ProdutionGenerator, Item>
    {
        public struct Ctx
        {
            public IResourceLoader resourceLoader;
            public ItemDataFactory itemDataFactory;
            public Inventory inventory;
        }
        
        private readonly Ctx _ctx;

        public ProductionGeneratorFactory(Ctx ctx)
        {
            _ctx = ctx;
        }

        public ProdutionGenerator CreateObject(Item asset)
        {
            ProdutionGenerator generator = null;
            switch (asset)
            {
                case SeedlingData:
                    generator =  new SoilPm(new SoilPm.Ctx
                    {
                        ProductionGeneratorFactory = this,
                        inventory = _ctx.inventory,
                    });
                    break;
                case GrapeData:
                    generator = new PlantPm(new PlantPm.Ctx
                    {
                        itemDataFactory = _ctx.itemDataFactory,
                        resourceLoader = _ctx.resourceLoader,
                    });
                    break;
                case JuiceData:
                    generator = new JuicerPm(new JuicerPm.Ctx
                    {
                        itemDataFactory = _ctx.itemDataFactory,
                        inventory = _ctx.inventory,
                    });
                    break;
                default:
                    Debug.Log($"Don't find asset with id {asset.Id}");
                    break;
            }

            return generator;
        }
    }
}