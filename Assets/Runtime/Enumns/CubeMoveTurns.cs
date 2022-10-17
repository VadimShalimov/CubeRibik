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
        AntiFront,

        UpForSide,
        DownForSide,
        LeftForSide,
        RightForSide,
        BackForSide,
        FrontForSide,

        AntiUpForSide,
        AntiDownForSide,
        AntiLeftForSide,
        AntiRightForSide,
        AntiBackForSide,
        AntiFrontForSide,
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
                    return new Vector3(1, 0, 0);
                case CubeMoveTurns.Front:
                    return new Vector3(-1, 0, 0);

                case CubeMoveTurns.AntiUp:
                    return new Vector3(0, -1, 0);
                case CubeMoveTurns.AntiDown:
                    return new Vector3(0, 1, 0);
                case CubeMoveTurns.AntiLeft:
                    return new Vector3(0, 0, 1);
                case CubeMoveTurns.AntiRight:
                    return new Vector3(0, 0, -1);
                case CubeMoveTurns.AntiBack:
                    return new Vector3(-1, 0, 0);
                case CubeMoveTurns.AntiFront:
                    return new Vector3(1, 0, 0);

                case CubeMoveTurns.UpForSide:
                    return new Vector3(0, 90, 0);
                case CubeMoveTurns.DownForSide:
                    return new Vector3(0, -90, 0);
                case CubeMoveTurns.LeftForSide:
                    return new Vector3(0, 0, -90);
                case CubeMoveTurns.RightForSide:
                    return new Vector3(0, 0, 90);
                case CubeMoveTurns.BackForSide:
                    return new Vector3(90, 0, 0);
                case CubeMoveTurns.FrontForSide:
                    return new Vector3(-90, 0, 0);


                case CubeMoveTurns.AntiUpForSide:
                    return new Vector3(0, -90, 0);
                case CubeMoveTurns.AntiDownForSide:
                    return new Vector3(0, 90, 0);
                case CubeMoveTurns.AntiLeftForSide:
                    return new Vector3(0, 0, 90);
                case CubeMoveTurns.AntiRightForSide:
                    return new Vector3(0, 0, -90);
                case CubeMoveTurns.AntiBackForSide:
                    return new Vector3(-90, 0, 0);
                case CubeMoveTurns.AntiFrontForSide:
                    return new Vector3(90, 0, 0);
            }

            return Vector3.zero;
        }
    }
}
