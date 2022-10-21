using UnityEngine;

namespace Assets.Runtime.Enums
{
    public enum CubeMoveTurns
    {
        CornerUp,
        CornerDown,
        CornerLeft,
        CornerRight,
        CornerBack,
        CornerFront,

        AntiCornerUp,
        AntiCornerDown,
        AntiCornerLeft,
        AntiCornerRight,
        AntiCornerBack,
        AntiCornerFront,
    }

    public static class CubeEnumnExtentions
    {
        public static Vector3 MoveTurnToVector(this CubeMoveTurns cubeMoveTurn)
        {
            switch (cubeMoveTurn)
            {
                case CubeMoveTurns.CornerUp:
                    return new Vector3(0, 90, 0);
                case CubeMoveTurns.CornerDown:
                    return new Vector3(0, -90, 0);
                case CubeMoveTurns.CornerLeft:
                    return new Vector3(0, 0, -90);
                case CubeMoveTurns.CornerRight:
                    return new Vector3(0, 0, 90);
                case CubeMoveTurns.CornerBack:
                    return new Vector3(90, 0, 0);
                case CubeMoveTurns.CornerFront:
                    return new Vector3(-90, 0, 0);


                case CubeMoveTurns.AntiCornerUp:
                    return new Vector3(0, -90, 0);
                case CubeMoveTurns.AntiCornerDown:
                    return new Vector3(0, 90, 0);
                case CubeMoveTurns.AntiCornerLeft:
                    return new Vector3(0, 0, 90);
                case CubeMoveTurns.AntiCornerRight:
                    return new Vector3(0, 0, -90);
                case CubeMoveTurns.AntiCornerBack:
                    return new Vector3(-90, 0, 0);
                case CubeMoveTurns.AntiCornerFront:
                    return new Vector3(90, 0, 0);
            }

            return Vector3.zero;
        }
    }
}
