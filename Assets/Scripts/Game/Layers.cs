using Core;

namespace Game
{
    public static class Layers
    {
        public static int GroundLayer = 7;
        public static int SelectableLayer = 3;

        public static int GroundMask => 1 << GroundLayer;
        public static int SelectablesMask => 1 << SelectableLayer;
    }
}