using System.Collections.Generic;

using Runtime.Enums;
using Runtime.Models;

namespace Runtime.Utils
{
    public class RotationHelper
    {
        private readonly CubeModel _cubeModel;

        public RotationHelper(CubeModel cubeModel)
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

            if (rotateCondition)
            {
                WriteReplaceableData(ref replaceableValues, sideIndexes, 0, 2, true);
                
                WriteReplaceableData(ref replaceableValues, sideIndexes, 1, 0);
                
                WriteReplaceableData(ref replaceableValues, sideIndexes, 3, 1, true, 1);

                WriteReplaceableData(ref replaceableValues, sideIndexes, 2, 3, false, 1);
            }
            else
            {
                WriteReplaceableData(ref replaceableValues, sideIndexes, 2, 0, true);
                
                WriteReplaceableData(ref replaceableValues, sideIndexes, 3, 2);
                
                WriteReplaceableData(ref replaceableValues, sideIndexes, 1, 3, true, 1);

                WriteReplaceableData(ref replaceableValues, sideIndexes, 0, 1, false, 1);
            }
            
            mainSide.PanelModel.ReplaceIndexes(replaceableValues);

            foreach (var connectedSideModel in mainSide.ConnectedSideModels)
            {
                connectedSideModel.PanelModel.ReplaceIndexes(replaceableValues);
            }
        }

        private void WriteReplaceableData(ref List<ReplaceableValue<int>> replaceableValues, int[][] sideIndexes,
            int startIndexSide, int lastIndexSide, bool reversible = false, int offset = 0)
        {
            if (reversible)
            {
                for (var i = 0; i < sideIndexes[startIndexSide].Length - 1; i++)
                {
                    var mainValue = sideIndexes[startIndexSide][i + offset];
                    var replaceValue = sideIndexes[lastIndexSide][sideIndexes[3].Length - 1 - i - offset];

                    replaceableValues.Add(new ReplaceableValue<int>(mainValue, replaceValue));
                }
            }
            else
            {
                for (var i = 0; i < sideIndexes[startIndexSide].Length - 1; i++)
                {
                    var mainValue = sideIndexes[startIndexSide][i + offset];
                    var replaceValue = sideIndexes[lastIndexSide][i + offset];

                    replaceableValues.Add(new ReplaceableValue<int>(mainValue, replaceValue));
                }
            }
        }
    }
}