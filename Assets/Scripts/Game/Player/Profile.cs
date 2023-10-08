using System.Collections.Generic;
using Core;
using Data;

namespace Game.Player
{
    public class Profile : BaseDisposable, IReadOnlyProfile
    {
        public struct Ctx
        {
            public Inventory inventory;
        }

        private readonly Ctx _ctx;
        
        public Profile(Ctx ctx)
        {
            _ctx = ctx;
        }


        public IInventory Inventory => _ctx.inventory;
    }
}