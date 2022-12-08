using System;
using System.Collections.Generic;
using System.Threading;

using Assets.Runtime.Configs;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using Runtime.Models;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Runtime.Core
{
    public class CubeViewFactory : IDisposable
    {
        private const int YPositionDivisorPower = 3;
        
        private readonly CubeModelGameplayConfig _gameplayConfig;
        private readonly CubeVisualConfig _visualConfig;
        private readonly InputModel _inputModel;

        private CancellationTokenSource _factoryCts;

        private double _delayTime;

        public CubeViewFactory(CubeModelGameplayConfig gameplayConfig, CubeVisualConfig visualConfig, InputModel inputModel)
        {
            _gameplayConfig = gameplayConfig;
            _visualConfig = visualConfig;
            _inputModel = inputModel;

            _factoryCts = new CancellationTokenSource();

            _delayTime = 0.5;
        }

        public async UniTask<IEnumerable<GameObject>> CreateViewsAsync()
        {
            var cubeViewObject = new GameObject();
            
            var cubeList = new List<GameObject>();

            for (var y = 0; y < _gameplayConfig.CubeSizeVector.y; y++)
            for (var z = 0; z < _gameplayConfig.CubeSizeVector.x; z++)
            for (var x = 0; x < _gameplayConfig.CubeSizeVector.x; x++)
            {
                var cube = await CreateAfterDelayAsync(cubeViewObject);
                cube.transform.localPosition = new Vector3(x, -y, -z);
                cubeList.Add(cube);
                
                await cube.transform.DOScale(Vector3.one, (float) _delayTime).WithCancellation(_factoryCts.Token);
            }

            var newCubePosition = new Vector3(cubeViewObject.transform.position.x, 
                (float)_gameplayConfig.CubeSizeVector.y / YPositionDivisorPower);

            await cubeViewObject.transform.DOMove(newCubePosition, 0.5f).WithCancellation(_factoryCts.Token);
            
            _inputModel.EnableInputFilter();
            
            return cubeList.ToArray();
        }

        private async UniTask<GameObject> CreateAfterDelayAsync(GameObject parent)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delayTime), cancellationToken: _factoryCts.Token);

            _delayTime /= 1.15;
            
            var gameObject = Object.Instantiate(_visualConfig.CubePrefab, parent.transform);
            
            gameObject.transform.localScale = Vector3.zero;

            return gameObject;
        }

        public void Dispose()
        {
            _factoryCts.Cancel();
            _factoryCts?.Dispose();
            _factoryCts = null;
        }
    }
}