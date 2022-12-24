using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Utils
{
    public class ViewRotationHelper
    {
        private CancellationTokenSource _animationTokenSource = new CancellationTokenSource();
        private GameObject _rotationRoot;
        private Tween _activeTween;

        public ViewRotationHelper()
        {
            _rotationRoot = new GameObject();
        }
        
        public async UniTask RotateSideAsync(GameObject[] cubes, RotateDirection direction)
        {
            int indexCentre = cubes.Length / 2;
            var cubeParent = cubes[indexCentre].transform.parent;

            _rotationRoot.transform.localPosition = cubes[indexCentre].transform.localPosition;

            foreach (var cube in cubes)
            {
                cube.transform.SetParent(_rotationRoot.transform);
            }

            _activeTween = _rotationRoot.transform.DORotate(direction.GetRotateDirection(), 0.5f, RotateMode.LocalAxisAdd);
            await _activeTween.WithCancellation(_animationTokenSource.Token);

            foreach (var cube in cubes)
            {
                cube.transform.SetParent(cubeParent);
            }
            
            _rotationRoot.transform.eulerAngles = Vector3.zero;
        }
    }
}