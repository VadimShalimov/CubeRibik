using System.Collections.Generic;

using Assets.Runtime.Configs;

using UnityEngine;

namespace Runtime.Core
{
    public class CubeViewFactory
    {
        private readonly CubeModelGameplayConfig _gameplayConfig;
        private readonly CubeVisualConfig _visualConfig;

        public CubeViewFactory(CubeModelGameplayConfig gameplayConfig, CubeVisualConfig visualConfig)
        {
            _gameplayConfig = gameplayConfig;
            _visualConfig = visualConfig;
        }

        public GameObject[] CreateViews()
        {
            var cubeList = new List<GameObject>();

            for (int x = 0; x < _gameplayConfig.CubeSizeVector.x; x++)
            for (int y = 0; y < _gameplayConfig.CubeSizeVector.y; y++)
            for (int z = 0; z < _gameplayConfig.CubeSizeVector.x; z++)
            {
                var cube = Object.Instantiate(_visualConfig.CubePrefab);
                cube.transform.localPosition = new Vector3(-x, -y, z);
                cubeList.Add(cube);
            }

            return cubeList.ToArray();
        }
    }
}