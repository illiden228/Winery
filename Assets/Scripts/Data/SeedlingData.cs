using Core;

namespace Data
{
    public class SeedlingData : Item
    {
        public SeedlingAsset Seedling;
        public float GrowthTime;
        public float FruitRipeTime;
        public int SproutStageCount;
        public int RipeStageCount;
        public ItemAsset Production;
        public string ViewPrefabName;
        public int ProductionCount;
    }
}