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

        public void RotateCubeModel(Side targetSide, bool directionCondition, int deep)
        {
            _cubeModel.RotateModel(targetSide, directionCondition, deep);
        }

        public Vector2 GetCubeSize()
        {
            return _cubeModel.CubeLenght;
        }
    }
}