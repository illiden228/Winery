using System.Collections.Generic;
using Core;
using Data;
using Game.Factories;
using Game.Selectables;
using UniRx;

namespace Core
{
    public class StartContextLoader : BaseDisposable
    {
        public struct Ctx
        {
            public ProductionGeneratorFactory ProductionGeneratorFactory;
            public List<SoilView> startSoils;
            public List<JuicerView> startJuicers;
            public List<BarrelView> startBarrels;
        }

        private readonly Ctx _ctx;
        
        public ReactiveCollection<SoilPm> StartSoils => GetStartSoils();
        public ReactiveCollection<JuicerPm> StartJuicers => GetStartJuicers();
        public ReactiveCollection<BarrelPm> StartBarrels => GetStartBarrels();

        public StartContextLoader(Ctx ctx)
        {
            _ctx = ctx;
        }

        private ReactiveCollection<SoilPm> GetStartSoils()
        {
            ReactiveCollection<SoilPm> soils = new ReactiveCollection<SoilPm>();
            foreach (var soilView in _ctx.startSoils)
            {
                SoilPm newSoil = (SoilPm) _ctx.ProductionGeneratorFactory.CreateObject(new SeedlingData());
                newSoil.InitView(soilView);
                soils.Add(newSoil);
            }

            return soils;
        }
        
        private ReactiveCollection<JuicerPm> GetStartJuicers()
        {
            ReactiveCollection<JuicerPm> juicers = new ReactiveCollection<JuicerPm>();
            foreach (var juiceView in _ctx.startJuicers)
            {
                JuicerPm newJuicer = (JuicerPm) _ctx.ProductionGeneratorFactory.CreateObject(new JuiceData());
                newJuicer.InitView(juiceView);
                juicers.Add(newJuicer);
            }

            return juicers;
        }
        
        private ReactiveCollection<BarrelPm> GetStartBarrels()
        {
            ReactiveCollection<BarrelPm> barrel = new ReactiveCollection<BarrelPm>();
            foreach (var barrelView in _ctx.startBarrels)
            {
                BarrelPm newBarrel = (BarrelPm) _ctx.ProductionGeneratorFactory.CreateObject(new WineData());
                newBarrel.InitView(barrelView);
                barrel.Add(newBarrel);
            }

            return barrel;
        }
    }
}