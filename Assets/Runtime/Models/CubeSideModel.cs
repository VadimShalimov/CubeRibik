using System;

using Assets.Runtime.Enums;

namespace Runtime.Models
{
    [Serializable]
    public struct CubeSideModel
    {
        public CubeSide LeftUpSide;

        public CubeSide UpSide;

        public CubeSide RightUpSide;

        public CubeSide LeftCenter;

        public CubeSide Center;

        public CubeSide RightCenter;

        public CubeSide LeftBottom;

        public CubeSide Bottom;

        public CubeSide RightBottom;
    }
}