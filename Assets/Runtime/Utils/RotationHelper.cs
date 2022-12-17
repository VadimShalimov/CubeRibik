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

        private void RotateMainSide(SideModel mainSide, bool rotateCondition)
        {
            var replaceableValues = new List<ReplaceableValue<int>>();

            var sideIndexes = mainSide.PanelModel.GetSidesIndexes();

            //todo: add rotation methods

            foreach (var connectedSideModel in mainSide.ConnectedSideModels)
            {
                connectedSideModel.PanelModel.ReplaceIndexes(replaceableValues);
            }
        }
    }
}