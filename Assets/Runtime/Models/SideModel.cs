using System.Collections.Generic;

using Runtime.Enums;

namespace Runtime.Models
{
    public class SideModel
    {
        public Side Side { get; }

        public CubePlacesModel PanelModel { get; }
        
        public List<CubePlacesModel> AttachedPanelModels { get; }

        public int MatrixLength => PanelModel.CubeIndexes.Length;

        public SideModel(Side side, int[] startIndexes)
        {
            Side = side;

            PanelModel = new CubePlacesModel(startIndexes);
        }

        public SideModel(Side side, int[] startIndexes, params int[][] attachedIndexes)
        {
            Side = side;

            PanelModel = new CubePlacesModel(startIndexes);

            AttachedPanelModels = new List<CubePlacesModel>(attachedIndexes.Length);

            foreach (var attached in attachedIndexes)
            {
                AttachedPanelModels.Add(new CubePlacesModel(attached));
            }
        }
    }
}