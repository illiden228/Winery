using System;
using Core;
using UnityEngine;

namespace Data
{
    [Serializable]
    public abstract class Item
    {
        public string Id;
        public int MaxCount;
        public int Count = 1;
        public Sprite Icon;
        public string Name;
        public int Cost;
        
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
            return id == Id;
        }
    }
}