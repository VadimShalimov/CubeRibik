using System.Numerics;
using Runtime.Enums;
using Runtime.Models;
using Vector2 = UnityEngine.Vector2;

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

        public Vector2 GetCubeSize()
        {
            return _cubeModel.CubeLenght;
        }
    }
}