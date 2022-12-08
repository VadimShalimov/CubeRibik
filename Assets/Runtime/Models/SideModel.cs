using Runtime.Enums;

namespace Runtime.Models
{
    public class SideModel
    {
        public Side Side { get; }

        public CubePlacesModel PanelModel { get; }
        
        public CubePlacesModel[] AttachedPanelModels { get; }

        public int MatrixLength => PanelModel.CubeIndexes.Length;

        public SideModel(Side side, int[] startIndexes)
        {
            Side = side;

            PanelModel = new CubePlacesModel(startIndexes);
        }

        public SideModel(Side side, int[] startIndexes, CubePlacesModel[] attachedIndexes)
        {
            Side = side;

            PanelModel = new CubePlacesModel(startIndexes);

            AttachedPanelModels = attachedIndexes;
        }
    }
}