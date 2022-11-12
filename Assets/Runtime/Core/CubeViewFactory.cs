using System.Collections.Generic;

using Assets.Runtime.Configs;

using UnityEngine;

namespace Runtime.Core
{
    public class CubeViewFactory
    {
        private const int YPositionDivisorPower = 3;
        
        private readonly CubeModelGameplayConfig _gameplayConfig;
        private readonly CubeVisualConfig _visualConfig;

        public CubeViewFactory(CubeModelGameplayConfig gameplayConfig, CubeVisualConfig visualConfig)
        {
            _gameplayConfig = gameplayConfig;
            _visualConfig = visualConfig;
        }

        public IEnumerable<GameObject> CreateViews()
        {
            var cubeViewObject = new GameObject();
            
            var cubeList = new List<GameObject>();

            for (var x = 0; x < _gameplayConfig.CubeSizeVector.x; x++)
            for (var y = 0; y < _gameplayConfig.CubeSizeVector.y; y++)
            for (var z = 0; z < _gameplayConfig.CubeSizeVector.x; z++)
            {
                var cube = Object.Instantiate(_visualConfig.CubePrefab, cubeViewObject.transform);
                cube.transform.localPosition = new Vector3(-x, -y, z);
                cubeList.Add(cube);
            }

            cubeViewObject.transform.position = new Vector3(cubeViewObject.transform.position.x, 
                (float)_gameplayConfig.CubeSizeVector.y / YPositionDivisorPower);

            return cubeList.ToArray();
        }
    }
}