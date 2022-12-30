using Runtime.Enums;

using UnityEngine;

namespace Runtime.Utils.Extensions
{
    public static class RotateDirectionExtension
    {
        public static Vector3 GetRotateDirection(this RotateDirection direction)
        {
            switch (direction)
            {
                case RotateDirection.YRight:
                    return Vector3.up * 90;
                case RotateDirection.YLeft:
                    return Vector3.down * 90;
                case RotateDirection.ZRight:
                    return Vector3.back * 90;
                case RotateDirection.ZLeft:
                    return Vector3.forward * 90;
                case RotateDirection.Up:
                    return Vector3.left * 90;
                case RotateDirection.Down:
                    return Vector3.right * 90;
                
            }
            return default;
        }

        public static RotateDirection GetRotateDirection(this Side side, bool rotateCondition)
        {
            switch (side)
            {
                case Side.BackSide:
                    return rotateCondition? RotateDirection.ZRight : RotateDirection.ZLeft;
                case Side.FrontSide:
                    return rotateCondition? RotateDirection.ZRight : RotateDirection.ZLeft;
                case Side.LeftSide:
                    return rotateCondition? RotateDirection.Up : RotateDirection.Down;
                case Side.RightSide:
                    return rotateCondition? RotateDirection.Up : RotateDirection.Down;
                case Side.UpSide:
                    return rotateCondition? RotateDirection.YRight : RotateDirection.YLeft;
                case Side.DownSide:
                    return rotateCondition? RotateDirection.YRight : RotateDirection.YLeft;
            }

            return default;
        }
    }
}