using Runtime.Enums;

namespace Runtime.Models
{
    public class SideModel
    {
        public Side Side { get; }

        public int[] CubeIndexes => _cubeIndexes;

        public int MatrixLength => _cubeIndexes.Length;

        private int[] _cubeIndexes;
        
        public SideModel(Side side, int[] startIndexes)
        {
            Side = side;

            _cubeIndexes = startIndexes;
        }
    }
}