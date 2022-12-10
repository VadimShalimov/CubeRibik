using Runtime.Enums;
using UnityEngine;

namespace Runtime.Models
{
    public class CubeModel
    {
        public Vector2 CubeLenght => _cubeLenght;
        
        private readonly SideModel[] _sidesData;
        private Vector2 _cubeLenght;
        
        public CubeModel(SideModel[] sidesData, Vector2 cubeLenght)
        {
            _sidesData = sidesData;
            _cubeLenght = cubeLenght;

            var cubeMatrixLength = 0;

            foreach (var sideData in _sidesData)
            {
                cubeMatrixLength += sideData.MatrixLength;
            }
        }

        public void RotateModel(Side rotatingSide, bool directionCondition, int deep)
        {
            Debug.Log($"target Side{rotatingSide} and direction condition{directionCondition} with {deep}");
        }
    }
}