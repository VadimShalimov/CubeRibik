using System.Collections.Generic;

using Runtime.Enums;
using Runtime.Models;

namespace Runtime.Utils
{
    public class ModelRotationHelper
    {
        private readonly CubeModel _cubeModel;

        public ModelRotationHelper(CubeModel cubeModel)
        {
            _cubeModel = cubeModel;
        }

        public void RotateCubeModel(Side side, bool rotateCondition, int nestedSidesDeep)
        {
            var targetSide = _cubeModel.GetSideModel(side);

            RotateMainSide(targetSide, rotateCondition);
        }

        //fixed attached indexes bug
        private void RotateMainSide(SideModel mainSide, bool rotateCondition)
        {
            var replaceableValues = new List<ReplaceableValue<int>>();

            var sideIndexes = mainSide.PanelModel.GetSidesIndexes();

            var sideMatrix = ArrayToMatrix(mainSide.PanelModel.CubeIndexes);

            if (rotateCondition)
            {
                RotateMatrixClockwise(sideMatrix, ref replaceableValues);
            }
            else
            {
                RotateMatrixAntiClockwise(sideMatrix, ref replaceableValues);
            }
            
            mainSide.PanelModel.ReplaceIndexes(replaceableValues);

            foreach (var connectedSideModel in mainSide.ConnectedSideModels)
            {
                connectedSideModel.PanelModel.ReplaceIndexes(replaceableValues);
            }
        }

        private void RotateMatrixClockwise(int[,] oldMatrix, ref List<ReplaceableValue<int>> replaceableValues)
        {
            int newColumn, newRow = 0;
            for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
            {
                newColumn = 0;
                for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
                {
                    var newValue = oldMatrix[newRow, newColumn];
                    var oldValue = oldMatrix[oldRow, oldColumn];
                    replaceableValues.Add(new ReplaceableValue<int>(oldValue, newValue));
                    newColumn++;
                }
                newRow++;
            }
        }
        
        private void RotateMatrixAntiClockwise(int[,] oldMatrix, ref List<ReplaceableValue<int>> replaceableValues)
        {
            int newColumn, newRow = 0;
            for (int oldColumn = 0; oldColumn < oldMatrix.GetLength(0) - 1; oldColumn++)
            {
                newColumn = 0;
                for (int oldRow = oldMatrix.GetLength(1) - 1 ; oldRow < 0 ; oldRow--)
                {
                    var newValue = oldMatrix[newRow, newColumn];
                    var oldValue = oldMatrix[oldRow, oldColumn];
                    replaceableValues.Add(new ReplaceableValue<int>(oldValue, newValue));
                    newColumn++;
                }
                newRow++;
            }
        }

        private int[,] ArrayToMatrix(int[] array)
        {
            var matrix = new int[_cubeModel.CubeLenght.x, _cubeModel.CubeLenght.y];

            var arrayIndex = 0;

            for (int i = 0; i < _cubeModel.CubeLenght.y; i++)
            {
                for (int j = 0; j < _cubeModel.CubeLenght.x; j++)
                {
                    matrix[i, j] = array[arrayIndex];

                    arrayIndex++;
                }
            }

            return matrix;
        }
    }
}