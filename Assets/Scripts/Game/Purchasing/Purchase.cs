using System;
using Core;
using Data;

namespace Game.Purchasing
{
    public struct Purchase
    {
        public Action Callback;
        public SeedlingData SeedlingData;
    }
}