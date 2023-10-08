using Core;

namespace Data
{
    public class SeedlingData : Item
    {
        public PlantAsset Plant;
        public float GrowthTime;
        public float FruitRipeTime;
        public int SproutStageCount;
        public int RipeStageCount;

        public bool TryAdd(string id, int count)
        {
            bool canAdd = CompareId(id);
            if (canAdd)
                Count += count;
            return canAdd;
        } 
        
        public bool TryRemove(string id, int count)
        {
            bool canRemove = CompareId(id) && count <= Count;
            if (canRemove)
                Count -= count;
            return canRemove;
        }

        public bool CompareId(string id)
        {
            return id == Plant.Id;
        }
    }
}