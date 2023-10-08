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
        //public AssetData Asset;
    }
}