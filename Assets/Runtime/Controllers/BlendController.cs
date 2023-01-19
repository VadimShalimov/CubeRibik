using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Enums;
using Runtime.Models;
using Runtime.Services;
using Runtime.Views;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Runtime.Controllers
{
    public class BlendController: IInitializable, IDisposable
    {
        private readonly Vector2 _minMaxBlendVector = new Vector2(3, 9);

        private readonly CubeInteractionService _cubeInteractionService;
        private readonly UIPlaceholderView _uiPlaceholderView;
        private readonly InputModel _inputModel;

        public BlendController(CubeInteractionService cubeInteractionService, UIPlaceholderView uiPlaceholderView, InputModel inputModel)
        {
            _cubeInteractionService = cubeInteractionService;
            _uiPlaceholderView = uiPlaceholderView;
            _inputModel = inputModel;
        }

        public void Initialize()
        {
            _uiPlaceholderView.ShuffleButtonClicked += ShuffleCube;
        }

        public void Dispose()
        {
            _uiPlaceholderView.ShuffleButtonClicked -= ShuffleCube;
        }
        
        private void ShuffleCube()
        {   
            if (!_inputModel.AvailableToInput) return;
            
            _inputModel.DisableInputFilter();
            
            var randomBlendActions = Random.Range(_minMaxBlendVector.x, _minMaxBlendVector.y);
            var valuesArray = new RotationValue[(int)randomBlendActions];
            
            for (int i = 0; i < randomBlendActions - 1; i++)
            {
                var randomValue = Random.Range(0, 7);
                var randomSide = (Side)randomValue;
                var randomCondition = Random.Range(0, 2) > 0;
                var randomDeep = Random.Range(0, _cubeInteractionService.GetNestedSideCount().y);

                valuesArray[i] = new RotationValue(randomSide, randomCondition, randomDeep);
            }
            BlendAsync(valuesArray).Forget();
        }

        private async UniTaskVoid BlendAsync(RotationValue[] rotationValues)
        {
            foreach (var rotationValue in rotationValues)
            {
                await _cubeInteractionService.RotateCubeModelAsync(rotationValue);
            }
            
            _inputModel.EnableInputFilter();
        }
    }
}