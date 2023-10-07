using Core;

namespace Game.Character
{
    public class CharacterView : BaseMonobehaviour
    {
        public struct Ctx
        {
        }

        private Ctx _ctx;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
        }

        
    }
}