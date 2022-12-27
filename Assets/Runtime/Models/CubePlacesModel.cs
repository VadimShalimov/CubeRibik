using System.Collections.Generic;
using System.Linq;

using Runtime.Utils;

using UnityEngine;

namespace Runtime.Models
{
    public class CubePlacesModel
    {
        public int[] CubeIndexes { get; private set; }

        private readonly Vector2Int _sideLength;

        public CubePlacesModel(int[] cubeIndexes, Vector2Int sideLength)
        {
            _sideLength = sideLength;
            CubeIndexes = cubeIndexes;
        }
        
        public void ReplaceIndexes(IReadOnlyList<ReplaceableValue<int>> replaceableValues)
        {
            for (var index = 0; index < CubeIndexes.Length; index++)
            {
                var replaceableValue = replaceableValues.FirstOrDefault(x => x.OldValue.Equals(CubeIndexes[index]));

                if (!replaceableValue.OldValue.Equals(CubeIndexes[index]))
                {
                    continue;
                }

                CubeIndexes[index] = replaceableValue.NewValue;
            }
        }
    }
}