using System;
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

        private readonly int _horizontalStep;

        public CubeModelFactory(CubeModelGameplayConfig gameplayConfig)
        {
            _gameplayConfig = gameplayConfig;

            _horizontalStep = (gameplayConfig.CubeSizeVector.x * gameplayConfig.CubeSizeVector.x);
        }

        public CubeModel CreateCubeModel()
        {
            var sidesModelList = new List<SideModel>(_gameplayConfig.SideConfigs.Length);

            var xNestedSidesCount = _gameplayConfig.CubeSizeVector.x <= 3
                ? 0
                : (int) Math.Round((decimal) _gameplayConfig.CubeSizeVector.x / 2 - 1, 0,
                    MidpointRounding.AwayFromZero);

            xNestedSidesCount = _gameplayConfig.CubeSizeVector.x % 2 == 0 ? xNestedSidesCount : xNestedSidesCount - 1;

            var yNestedSidesCount = _gameplayConfig.CubeSizeVector.y <= 3
                ? 0
                : (int) Math.Round((decimal) _gameplayConfig.CubeSizeVector.y / 2 - 1, 0,
                    MidpointRounding.AwayFromZero);

            yNestedSidesCount = _gameplayConfig.CubeSizeVector.y % 2 == 0 ? yNestedSidesCount : yNestedSidesCount - 1;

            var upperSideModel = GenerateDefaultSideModel(_gameplayConfig.SideConfigs.First(), 0, 1, yNestedSidesCount);

            sidesModelList.Add(upperSideModel);

            var sidesStartIndexes = new List<int[]>(4)
            {
                GetSideIndexes(upperSideModel.PanelModel.CubeIndexes),
                GetSideIndexes(upperSideModel.PanelModel.CubeIndexes, true, _gameplayConfig.CubeSizeVector.x - 1),
                GetSideIndexes(
                    upperSideModel.PanelModel.CubeIndexes
                        .Skip(upperSideModel.PanelModel.CubeIndexes.Length - _gameplayConfig.CubeSizeVector.x)
                        .ToArray(), false),
                GetSideIndexes(upperSideModel.PanelModel.CubeIndexes, false),
            };

            for (var index = 1; index < _gameplayConfig.SideConfigs.Length - 1; index++)
            {
                var sideConfig = _gameplayConfig.SideConfigs[index];

                var indexes = sidesStartIndexes[index - 1].ToList();

                var currentIndex = 0;

                while (indexes.Count < sideConfig.ColorsArray.Length)
                {
                    indexes.Add(indexes[currentIndex] +
                                (_gameplayConfig.CubeSizeVector.x * _gameplayConfig.CubeSizeVector.x));

                    currentIndex++;
                }

                var newSideModel =
                    new SideModel(sideConfig.CubeSide, indexes.ToArray(), _gameplayConfig.CubeSizeVector);

                var nestedSides = new List<CubePlacesModel>();

                if (index % 2 == 0)
                {
                    for (var i = 0; i < xNestedSidesCount; i++)
                    {
                        nestedSides.Add(GenerateNestedSideModel(newSideModel.PanelModel, -1, xNestedSidesCount));
                    }
                }
                else
                {
                    for (var i = 0; i < xNestedSidesCount; i++)
                    {
                        nestedSides.Add(GenerateNestedSideModel(newSideModel.PanelModel, 1, xNestedSidesCount));
                    }
                }

                sidesModelList.Add(new SideModel(newSideModel.Side, newSideModel.PanelModel.CubeIndexes,
                    nestedSides.ToArray(), _gameplayConfig.CubeSizeVector));
            }

            var downSideStartIndex = (_gameplayConfig.CubeSizeVector.x * _gameplayConfig.CubeSizeVector.x) *
                                     (_gameplayConfig.CubeSizeVector.y - 1);

            var lastSideModel = GenerateDefaultSideModel(_gameplayConfig.SideConfigs.Last(), downSideStartIndex, -1,
                yNestedSidesCount);

            sidesModelList.Add(lastSideModel);

            return new CubeModel(sidesModelList.ToArray(), _gameplayConfig.CubeSizeVector);
        }

        private SideModel GenerateDefaultSideModel(SideConfig sideConfig, int startIndex, int offset,
            int nestedSidesCount = 0)
        {
            var indexes = new int[sideConfig.ColorsArray.Length];

            for (var index = 0; index < sideConfig.ColorsArray.Length; index++)
            {
                indexes[index] = startIndex;

                startIndex++;
            }

            var sideModel = new CubePlacesModel(indexes, _gameplayConfig.CubeSizeVector);

            var nestedSides = new List<CubePlacesModel>();

            for (var i = 0; i < nestedSidesCount; i++)
            {
                nestedSides.Add(GenerateNestedSideModel(sideModel, (_horizontalStep * offset) + (i * offset),
                    nestedSidesCount));
            }

            return new SideModel(sideConfig.CubeSide, indexes, nestedSides.ToArray(), _gameplayConfig.CubeSizeVector);
        }

        private CubePlacesModel GenerateNestedSideModel(CubePlacesModel previousCollectionModel, int offset,
            int nestedSidesCount)
        {
            if (nestedSidesCount <= 0)
            {
                return null;
            }

            var cubeIndexes = new List<int>();

            foreach (var index in previousCollectionModel.CubeIndexes)
            {
                cubeIndexes.Add(index + offset);
            }

            return new CubePlacesModel(cubeIndexes.ToArray(), _gameplayConfig.CubeSizeVector);
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