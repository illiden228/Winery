using Core;
using UnityEngine.UIElements.Experimental;

namespace Game.Character
{
    public static class CharacterAnimation
    {
        public static class Triggers
        {
            public const string Take = "Take";
            public const string Idle = "Idle";
            public const string Dig = "Dig";
            public const string Collect = "Collect";
        }
        public static class Bool
        {
            public static string Move = "Move";
        }
    }
}