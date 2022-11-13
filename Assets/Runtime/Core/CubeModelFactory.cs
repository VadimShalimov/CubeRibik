using System.Collections.Generic;
using System.Linq;

using Assets.Runtime.Configs;
using Assets.Runtime.Configs.SerializableObjects;

using Runtime.Models;

namespace Runtime.Core
{
    public class CubeModelFactory
    {
        private readonly CubeModelGameplayConfig _gameplayConfig;

        public CubeModelFactory(CubeModelGameplayConfig gameplayConfig)
        {
            _gameplayConfig = gameplayConfig;
        }

        public CubeModel CreateCubeModel()
        {
            var sidesModelList = new List<SideModel>(_gameplayConfig.SideConfigs.Length);

            var upperSideModel = GenerateDefaultSideModel(_gameplayConfig.SideConfigs.First(), 0);
            
            sidesModelList.Add(upperSideModel);
            
            var sidesStartIndexes = new List<int[]>(4)
            {
                GetSideIndexes(upperSideModel.PanelModel.CubeIndexes),
                GetSideIndexes(upperSideModel.PanelModel.CubeIndexes, true ,_gameplayConfig.CubeSizeVector.x - 1),
                GetSideIndexes(upperSideModel.PanelModel.CubeIndexes.Skip(_gameplayConfig.CubeSizeVector.x * 2).ToArray(), false),
                GetSideIndexes(upperSideModel.PanelModel.CubeIndexes, false),
            };

            for (var index = 1; index < _gameplayConfig.SideConfigs.Length - 1; index++)
            {
                var sideConfig = _gameplayConfig.SideConfigs[index];

                var indexes = sidesStartIndexes[index - 1].ToList();

                var currentIndex = 0;

                while (indexes.Count < sideConfig.ColorsArray.Length)
                {
                    indexes.Add(indexes[currentIndex] + (_gameplayConfig.CubeSizeVector.x * _gameplayConfig.CubeSizeVector.x));

                    currentIndex++;
                }
                
                sidesModelList.Add(new SideModel(sideConfig.CubeSide, indexes.ToArray()));
            }

            var downSideStartIndex = (_gameplayConfig.CubeSizeVector.x * _gameplayConfig.CubeSizeVector.x) * (_gameplayConfig.CubeSizeVector.y - 1);

            var lastSideModel = GenerateDefaultSideModel(_gameplayConfig.SideConfigs.Last(), downSideStartIndex);
            
            sidesModelList.Add(lastSideModel);

            return new CubeModel(sidesModelList.ToArray());
        }

        private SideModel GenerateDefaultSideModel(SideConfig sideConfig, int startIndex)
        {
            var indexes = new int[sideConfig.ColorsArray.Length];

            for (var index = 0; index < sideConfig.ColorsArray.Length; index++)
            {
                indexes[index] = startIndex;

                startIndex++;
            }

            return new SideModel(sideConfig.CubeSide, indexes);
        }

        private int[] GetSideIndexes(IReadOnlyList<int> startModel, bool isDefaultPattern = true, int startIndex = 0)
        {
            if (isDefaultPattern)
            {
                var indexes = new List<int> {startModel[startIndex]};

                for (int i = 0; i < _gameplayConfig.CubeSizeVector.x - 1; i++)
                {
                    indexes.Add(indexes[i] + _gameplayConfig.CubeSizeVector.x);
                }

                return indexes.ToArray();
            }

            return startModel.Take(_gameplayConfig.CubeSizeVector.x).ToArray();
        }
    }
}