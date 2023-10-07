using Core;
using UniRx;

namespace Game.Character
{
    public class CharacterModel
    {
        private bool _isMove;
        private float _speed;
        
        public ReactiveProperty<bool> IsMove;
        public ReactiveProperty<float> Speed;
        
        public CharacterModel()
        {
            IsMove = new ReactiveProperty<bool>(false);
            Speed = new ReactiveProperty<float>();
        }
    }
}