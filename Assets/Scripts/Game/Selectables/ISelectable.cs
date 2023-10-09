using Data;

namespace Game.Selectables
{
    public interface ISelectable
    {
        public void Activate(Item item);
        public SelectableStatus GetSelectState();
    }
}