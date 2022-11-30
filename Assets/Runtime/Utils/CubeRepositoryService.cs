using Runtime.Enums;
using Runtime.Models;

namespace Runtime.Utils
{
    public class CubeRepositoryService
    {
        private CubeModel _cubeModel;

        public void AddCubeModel(CubeModel cubeModel)
        {
            _cubeModel = cubeModel;
        }

        public void RotateCubeModel(Side targetSide, bool directionCondition)
        {
            _cubeModel.RotateModel(targetSide, directionCondition);
        }
    }
}