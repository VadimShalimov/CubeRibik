using System.Collections.Generic;
using System.Linq;

using Assets.Runtime.Configs.SerializableObjects;

using Runtime.Enums;
using Runtime.Models;

using Unity.Collections;

using UnityEngine;

namespace Assets.Runtime.Configs
{
    [CreateAssetMenu(fileName = nameof(CubeModelGameplayConfig), menuName = "MENUNAME", order = 0)]
    public class CubeModelGameplayConfig : ScriptableObject
    {
        [SerializeField] [ReadOnly] private SideConfig[] _sidesConfigs;
        
        [SerializeField] private int _xzDimensionalLength = 3;

        [SerializeField] private int _yDimensionalLength = 3;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_sidesConfigs.Length == 0)
            {
                _sidesConfigs = GenerateStartConfig();
            }
            
            if (_sidesConfigs != null)
            {
                _sidesConfigs[0].SetArrayLength(_xzDimensionalLength, _xzDimensionalLength);

                for (var index = 1; index < _sidesConfigs.Length - 1; index++)
                {
                    var side = _sidesConfigs[index];
                    side.SetArrayLength(_xzDimensionalLength, _yDimensionalLength);
                }
                
                _sidesConfigs[5].SetArrayLength(_xzDimensionalLength, _xzDimensionalLength);
            }
        }

        private SideConfig[] GenerateStartConfig()
        {
            return new[]
            {
                new SideConfig(Side.UpSide, CubeColors.White),
                new SideConfig(Side.LeftSide, CubeColors.Blue),
                new SideConfig(Side.RightSide, CubeColors.Green),
                new SideConfig(Side.FrontSide, CubeColors.Red),
                new SideConfig(Side.BackSide, CubeColors.Orange),
                new SideConfig(Side.DownSide, CubeColors.Yellow),
            };
        }
#endif

        public CubeModel CreateCubeModel()
        {
            var sidesModelList = new List<SideModel>(_sidesConfigs.Length);

            var upperSideModel = GenerateDefaultSideModel(_sidesConfigs.First(), 0);
            
            sidesModelList.Add(upperSideModel);
            
            var sidesStartIndexes = new List<int[]>(4)
            {
                GetSideIndexes(upperSideModel.CubeIndexes),
                GetSideIndexes(upperSideModel.CubeIndexes, true ,_xzDimensionalLength - 1),
                GetSideIndexes(upperSideModel.CubeIndexes.Skip(_xzDimensionalLength * 2).ToArray(), false),
                GetSideIndexes(upperSideModel.CubeIndexes, false),
            };

            for (var index = 1; index < _sidesConfigs.Length - 1; index++)
            {
                var sideConfig = _sidesConfigs[index];

                var indexes = sidesStartIndexes[index].ToList();

                var currentIndex = 0;

                while (indexes.Count < sideConfig.ColorsArray.Length)
                {
                    indexes.Add(indexes[currentIndex] + _xzDimensionalLength);

                    currentIndex++;
                }
                
                sidesModelList.Add(new SideModel(sideConfig.CubeSide, indexes.ToArray()));
            }

            var downSideStartIndex = (_xzDimensionalLength * _xzDimensionalLength) * (_yDimensionalLength - 1) - 1;

            var lastSideModel = GenerateDefaultSideModel(_sidesConfigs.Last(), downSideStartIndex);
            
            sidesModelList.Add(lastSideModel);

            return new CubeModel(sidesModelList.ToArray());
        }

        private SideModel GenerateDefaultSideModel(SideConfig sideConfig, int startIndex)
        {
            var indexes = new int[sideConfig.ColorsArray.Length];

            for (var index = startIndex; index < sideConfig.ColorsArray.Length; index++)
            {
                indexes[index] = index;
            }

            return new SideModel(sideConfig.CubeSide, indexes);
        }

        private int[] GetSideIndexes(IReadOnlyList<int> startModel, bool isDefaultPattern = true, int startIndex = 0)
        {
            if (isDefaultPattern)
            {
                var indexes = new List<int> {startModel[startIndex]};

                for (int i = 0; i < _xzDimensionalLength; i++)
                {
                    indexes.Add(indexes[i] * _xzDimensionalLength);
                }

                return indexes.ToArray();
            }

            return startModel.Take(_xzDimensionalLength).ToArray();
        }
    }
}