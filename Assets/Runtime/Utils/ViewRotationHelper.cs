using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using Runtime.Enums;
using Runtime.Utils.Extensions;

using UnityEngine;

namespace Runtime.Utils
{
    public class ViewRotationHelper : IDisposable
    {
        private const float RotateDuration = 0.5f;

        private readonly GameObject _rotationRoot;

        private Tween _activeTween;

        private CancellationTokenSource _animationTokenSource = new CancellationTokenSource();

        public ViewRotationHelper()
        {
            _rotationRoot = new GameObject();
        }

        public void Dispose()
        {
            _activeTween.KillActive();

            CtsExtensions.CancelToken(ref _animationTokenSource);
        }

        public async UniTask RotateSideAsync(GameObject[] cubes, RotateDirection direction, int deep)
        {
            if (_activeTween.IsActive())
            {
                return;
            }

            var indexCentre = (cubes.Length / deep) / 2;
            var cubeParent = cubes[indexCentre].transform.parent;

            _rotationRoot.transform.position = cubes[indexCentre].transform.position;

            foreach (var cube in cubes)
            {
                cube.transform.SetParent(_rotationRoot.transform);
            }

            _activeTween.KillActive();

            _activeTween =
                _rotationRoot.transform.DOLocalRotate(direction.GetRotateDirection(), RotateDuration,
                    RotateMode.WorldAxisAdd);

            await _activeTween.WithCancellation(_animationTokenSource.Token);

            foreach (var cube in cubes)
            {
                cube.transform.SetParent(cubeParent);
            }

            _rotationRoot.transform.eulerAngles = Vector3.zero;
        }
    }
}