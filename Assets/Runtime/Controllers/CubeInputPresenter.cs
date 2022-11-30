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

        private int x;
        private int y;

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
        }
        
        public void Dispose()
        {
            _inputModel.HorizontalSideChanged -= HandleHorizontalSideChanged;
            _inputModel.VerticalSideChanged -= HandleVerticalSideChanged;
            _inputModel.RotateAction -= HandleRotateAction;
        }

        private void HandleVerticalSideChanged(int obj)
        {
            y+= obj;
            Debug.Log(nameof(HandleVerticalSideChanged) + ": " + y);
        }

        private void HandleHorizontalSideChanged(int obj)
        {
            x+= obj;
            Debug.Log(nameof(HandleHorizontalSideChanged)+ ": " + x);
        }

        private void HandleRotateAction(bool condition)
        {
            _cubeRepositoryService.RotateCubeModel(Side.RightSide, condition);
        }
    }
}