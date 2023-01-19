using Runtime.Enums;

namespace Runtime.Models
{
    public struct RotationValue
    {
        public readonly Side RotationSide;
        public readonly bool RotationCondition;
        public readonly int Deep;

        public RotationValue(Side rotationSide, bool rotationCondition, int deep)
        {
            RotationSide = rotationSide;
            RotationCondition = rotationCondition;
            Deep = deep;
        }
    }
}