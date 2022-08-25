using UnityEngine;

namespace Assets.Runtime.Enumns
{
    public enum CubeMoveTurns
    {
        Up,
        Down,
        Left,
        Right,
        Back,
        Front,

        AntiUp,
        AntiDown,
        AntiLeft,
        AntiRight,
        AntiBack,
        AntiFront
    }

    public static class CubeEnumnExtentions
    {
        public static Vector3 MoveTurnToVector(this CubeMoveTurns cubeMoveTurn)
        {
            switch (cubeMoveTurn)
            {
                case CubeMoveTurns.Up:
                    return new Vector3(0, 1, 0);
                case CubeMoveTurns.Down:
                    return new Vector3(0, -1, 0);
                case CubeMoveTurns.Left:
                    return new Vector3(0, 0, -1);
                case CubeMoveTurns.Right:
                    return new Vector3(0, 0, 1);
                case CubeMoveTurns.Back:
                    return new Vector3(-1, 0, 0);
                case CubeMoveTurns.Front:
                    return new Vector3(1, 0, 0);

                case CubeMoveTurns.AntiUp:
                    return new Vector3(0, -1, 0);
                case CubeMoveTurns.AntiDown:
                    return new Vector3(0, 1, 0);
                case CubeMoveTurns.AntiLeft:
                    return new Vector3(0, 0, 1);
                case CubeMoveTurns.AntiRight:
                    return new Vector3(0, 0, -1);
                case CubeMoveTurns.AntiBack:
                    return new Vector3(1, 0, 0);
                case CubeMoveTurns.AntiFront:
                    return new Vector3(-1, 0, 0);

            }

            return Vector3.zero;
        }
    }
}
