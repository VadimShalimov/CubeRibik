using Runtime.Enums;

using UnityEngine;

namespace Runtime.Models
{
    public class SideModel
    {
        public Side Side { get; }

        public CubePlacesModel PanelModel { get; }
        
        public CubePlacesModel[] AttachedPanelModels { get; }
        
        public SideModel[] ConnectedSideModels { get; private set; }

        public int MatrixLength => PanelModel.CubeIndexes.Length;

        public SideModel(Side side, int[] startIndexes, Vector2Int sideLength)
        {
            Side = side;

            PanelModel = new CubePlacesModel(startIndexes, sideLength);
        }

        public SideModel(Side side, int[] startIndexes, CubePlacesModel[] attachedIndexes, Vector2Int sideLength)
        {
            Side = side;

            PanelModel = new CubePlacesModel(startIndexes, sideLength);

            AttachedPanelModels = attachedIndexes;
        }

        public void SetConnectedSideModels(params SideModel[] connectedSides)
        {
            ConnectedSideModels = connectedSides;
        }
    }
}