using System;
using Runtime.Enums;
using Runtime.Models;
using Runtime.Utils;
using UnityEngine;
using VContainer.Unity;

namespace Runtime.Controllers
{
    public class CubeInputPresenter: IInitializable, IDisposable
    {
        private readonly InputModel _inputModel;
        private readonly CubeRepositoryService _cubeRepositoryService;

        private Side _currentSide;
        private Vector2 _cubeSize;
        private int _x;
        private int _y;
        private int _deep;
        
        public CubeInputPresenter(InputModel inputModel, CubeRepositoryService cubeRepositoryService)
        {
            _inputModel = inputModel;
            _cubeRepositoryService = cubeRepositoryService;
        }

        public void Initialize()
        {
            _inputModel.HorizontalSideChanged += HandleHorizontalSideChanged;
            _inputModel.VerticalSideChanged += HandleVerticalSideChanged;
            _inputModel.RotateAction += HandleRotateAction;
            _cubeSize = _cubeRepositoryService.GetCubeSize();
        }
        
        public void Dispose()
        {
            _inputModel.HorizontalSideChanged -= HandleHorizontalSideChanged;
            _inputModel.VerticalSideChanged -= HandleVerticalSideChanged;
            _inputModel.RotateAction -= HandleRotateAction;
        }

        private void HandleVerticalSideChanged(int obj)
        {
            HandleSideChanged(obj, ref _y, _cubeSize.y, Side.UpSide, Side.DownSide);
        }
        
        private void HandleHorizontalSideChanged(int obj)
        {
            HandleSideChanged(obj,ref _x, _cubeSize.x, Side.LeftSide, Side.RightSide);
        }
        
        private void HandleSideChanged(int count, ref int os, float size, Side side, Side antiSide)
        {
            os += count;
            
            if (os > size)
            {
                os = 1;
            }

            if (os < 1)
            {
                os = (int)size;
            }

            if (os <= size % 2)
            {
                _deep = os;
                
                _currentSide = side;

                return;
            }

            _deep = ((int)size - os) + 1;
            _currentSide = antiSide;
        }

        private void HandleRotateAction(bool condition)
        {
            _cubeRepositoryService.RotateCubeModel(_currentSide, condition, _deep);
        }
    }
}