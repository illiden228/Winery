using Core;
using Data;
using Tools.Extensions;
using UniRx;

namespace Factories
{
    public class ItemDataFactory : BaseDisposable
    {
        public struct Ctx
        {
             
        }

        private readonly Ctx _ctx;

        public ItemDataFactory(Ctx ctx)
        {
            _ctx = ctx;
        }

        // public Item CreateItem(AssetData asset)
        // {
        //     switch ()
        //     {
        //         
        //     }
        // }
    }
}