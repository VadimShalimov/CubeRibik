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

                CubeIndexes[index] = replaceableValue.NewValue;
            }
        }

        public int[][] GetSidesIndexes()
        {
            return new[]
            {
                GetSideIndexes(false),
                GetSideIndexes(true, _sideLength.x - 1),
                GetSideIndexes(),
                GetSideIndexes(false, _sideLength.x * _sideLength.y - 1)

            };
        } 
        
        private int[] GetSideIndexes(bool isDefaultPattern = true, int startIndex = 0)
        {
            if (isDefaultPattern)
            {
                var indexes = new List<int> {startIndex};

                for (int i = 0; i < _sideLength.x - 1; i++)
                {
                    indexes.Add(indexes[i] + _sideLength.x);
                }

                return indexes.ToArray();
            }
            else
            {
                var indexes = new List<int> {startIndex};
                
                for (int i = indexes.First(); i < CubeIndexes.Length - 1; i++)
                {
                    indexes.Add(i);
                }
            }

            return null;
        }
    }
}