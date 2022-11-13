namespace Runtime.Models
{
    public class CubePlacesModel
    {
        public int[] CubeIndexes { get; private set; }

        public CubePlacesModel(int[] cubeIndexes)
        {
            CubeIndexes = cubeIndexes;
        }
    }
}