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

        public void RotateCubeModel(Side side, bool rotateCondition, int sidesDeep)
        {
            var targetSide = _cubeModel.GetSideModel(side);

            for (int i = 1; i < sidesDeep + 1; i++)
            {
                RotateSide(targetSide, rotateCondition, i);
            }
        }

        private void RotateSide(SideModel sideModel, bool rotateCondition, int deep)
        {
            var replaceableValues = new List<ReplaceableValue<int>>();

            var placesModel = deep <= 1 ? sideModel.PanelModel : sideModel.AttachedPanelModels[deep - 2];

            var sideMatrix = ArrayToMatrix(placesModel.CubeIndexes);

            if (rotateCondition)
            {
                RotateMatrix(sideMatrix, ref replaceableValues,true);
            }
            else
            {
                RotateMatrix(sideMatrix, ref replaceableValues, false);
            }

            placesModel.ReplaceIndexes(replaceableValues);

            RewriteGraphs(sideModel, replaceableValues);
        }

        private void RotateMatrix(int[,] matrix, ref List<ReplaceableValue<int>> replaceableValues,
            bool rotateCondition)
        {
            var newRow = 0;
            var newColumn = 0;

            if (rotateCondition)
            {
                for (int oldColumn = matrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
                {
                    newColumn = 0;
                    for (int oldRow = 0; oldRow < matrix.GetLength(0); oldRow++)
                    {
                        AddReplaceableData(matrix, newRow, newColumn, oldRow, oldColumn, ref replaceableValues);

                        newColumn++;
                    }

                    newRow++;
                }
            }
            else
            {
                for (int oldColumn = 0; oldColumn < matrix.GetLength(1); oldColumn++)
                {
                    newColumn = 0;
                    for (int oldRow = matrix.GetLength(1) - 1; oldRow >= 0; oldRow--)
                    {
                        AddReplaceableData(matrix, newRow, newColumn, oldRow, oldColumn, ref replaceableValues);
                        
                        newColumn++;
                    }

                    newRow++;
                }
            }
        }
        
        private void RewriteGraphs(SideModel mainSide, IReadOnlyList<ReplaceableValue<int>> replaceableValues)
        {
            foreach (var connectedSideModel in mainSide.ConnectedSideModels)
            {
                connectedSideModel.PanelModel.ReplaceIndexes(replaceableValues);

                foreach (var attachedPanelModel in connectedSideModel.AttachedPanelModels)
                {
                    attachedPanelModel.ReplaceIndexes(replaceableValues);
                }
            }
        }

        private void AddReplaceableData(int[,] matrix, int newRow, int newColumn, int oldRow, int oldColumn,
            ref List<ReplaceableValue<int>> replaceableValues)
        {
            var newValue = matrix[newRow, newColumn];
            var oldValue = matrix[oldRow, oldColumn];

            replaceableValues.Add(new ReplaceableValue<int>(oldValue, newValue));
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