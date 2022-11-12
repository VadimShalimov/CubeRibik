using Assets.Runtime.Configs.SerializableObjects;

namespace Runtime.Models
{
    public class CubeModel
    {
        private readonly SideModel[] _sidesData;

        public CubeModel(SideModel[] sidesData)
        {
            _sidesData = sidesData;

            var cubeMatrixLength = 0;

            foreach (var sideData in _sidesData)
            {
                cubeMatrixLength += sideData.MatrixLength;
            }
        }
    }
}