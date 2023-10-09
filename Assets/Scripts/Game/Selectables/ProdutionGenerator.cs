using Core;
using Data;

namespace Game.Selectables
{
    public abstract class ProdutionGenerator : BaseDisposable
    {
        public abstract void StartGeneration(Item to, Item from = null);
    }
}