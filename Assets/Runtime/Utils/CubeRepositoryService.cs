using Runtime.Enums;
using Runtime.Models;
using Vector2 = UnityEngine.Vector2;

namespace Runtime.Utils
{
    public class CubeRepositoryService
    {
        private CubeModel _cubeModel;

        private RotationHelper _rotationHelper;
        
        public void AddCubeModel(CubeModel cubeModel)
        {
            _cubeModel = cubeModel;

            _rotationHelper = new RotationHelper(_cubeModel);
        }

        public void RotateCubeModel(Side targetSide, bool directionCondition, int deep)
        {
            _rotationHelper.RotateCubeModel(targetSide, directionCondition, deep);
        }

        public Vector2 GetCubeSize()
        {
            return _cubeModel.CubeLenght;
        }
    }
}