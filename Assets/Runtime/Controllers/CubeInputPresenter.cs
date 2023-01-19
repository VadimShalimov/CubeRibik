using System;

using Runtime.Enums;
using Runtime.Models;
using Runtime.Services;

using UnityEngine;

using VContainer.Unity;

namespace Runtime.Controllers
{
    public class CubeInputPresenter : IInitializable, IDisposable
    {
        private readonly InputModel _inputModel;
        private readonly CubeInteractionService _cubeInteractionService;

        private Side _currentSide;

        private Vector2 _cubeSize;

        private int _x;
        private int _y;
        private int _z;

        private int _deep;

        public CubeInputPresenter(InputModel inputModel, CubeInteractionService cubeInteractionService)
        {
            _inputModel = inputModel;
            _cubeInteractionService = cubeInteractionService;
            _deep = 1;
        }

        public void Initialize()
        {
            _inputModel.HorizontalSideChanged += HandleHorizontalSideChanged;
            _inputModel.VerticalSideChanged += HandleVerticalSideChanged;
            _inputModel.ZSideChanged += HandleZSideChanged;
            _inputModel.RotateAction += HandleRotateAction;
            _cubeSize = _cubeInteractionService.GetCubeSize();
        }

        public void Dispose()
        {
            _inputModel.HorizontalSideChanged -= HandleHorizontalSideChanged;
            _inputModel.VerticalSideChanged -= HandleVerticalSideChanged;
            _inputModel.ZSideChanged -= HandleZSideChanged;
            _inputModel.RotateAction -= HandleRotateAction;
        }

        private void HandleVerticalSideChanged(int obj)
        {
            HandleSideChanged(obj, ref _y, _cubeSize.y, Side.UpSide, Side.DownSide);
        }

        private void HandleHorizontalSideChanged(int obj)
        {
            HandleSideChanged(obj, ref _x, _cubeSize.x, Side.LeftSide, Side.RightSide);
        }

        private void HandleZSideChanged(int obj)
        {
            HandleSideChanged(obj, ref _z, _cubeSize.x, Side.FrontSide, Side.BackSide);
        }

        private void HandleSideChanged(int valueAmount, ref int axisValue, float axisLength, Side firstSide,
            Side secondSide)
        {
            axisValue += valueAmount;

            if (axisValue > axisLength)
            {
                axisValue = 1;
            }

            if (axisValue < 1)
            {
                axisValue = (int) axisLength;
            }

            if (axisLength % 2 != 0)
            {
                if (axisValue == (int) axisLength / 2 + 1)
                {
                    axisValue += valueAmount;
                }
            }

            if (axisValue <= (int) axisLength / 2)
            {
                _deep = axisValue;

                _currentSide = firstSide;

                return;
            }

            _deep = ((int) axisLength - axisValue) + 1;
            _currentSide = secondSide;
        }

        private void HandleRotateAction(bool condition)
        {
            _cubeInteractionService.RotateCubeModelAsync(new RotationValue(_currentSide, condition, _deep));
        }
    }
}