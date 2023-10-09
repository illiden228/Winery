using Core;
using Game.Selectables;

namespace Game.Factories
{
    public interface IFactory<T1, T2>
    {
        public T1 CreateObject(T2 asset);
    }
}