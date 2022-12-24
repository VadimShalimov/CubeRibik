using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Enums;
using Runtime.Models;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Runtime.Utils
{
    public class CubeRepositoryService
    {
        private CubeModel _cubeModel;

        private ModelRotationHelper _modelRotationHelper;

        private GameObject[] _gameObjects;

        private ViewRotationHelper _viewRotationHelper;
        public void AddCubeModel(CubeModel cubeModel)
        {
            _cubeModel = cubeModel;

            _modelRotationHelper = new ModelRotationHelper(_cubeModel);
        }

        public void AddCubeViews(GameObject[] views)
        {
            _gameObjects = views;
            _viewRotationHelper = new ViewRotationHelper();
        }

        public void RotateCubeModel(Side targetSide, bool directionCondition, int deep)
        {
            var cubeList = new List<GameObject>();
            var cubeIndexes = _cubeModel.GetSideModel(targetSide).PanelModel.CubeIndexes;

            foreach (var cubeIndex in cubeIndexes)
            {
                cubeList.Add(_gameObjects[cubeIndex]);
            }
            
            _viewRotationHelper.RotateSideAsync(cubeList.ToArray(), targetSide.GetRotateDirection(directionCondition)).Forget();
            Debug.Log(targetSide);
            _modelRotationHelper.RotateCubeModel(targetSide, directionCondition, deep);
        }

        public Vector2 GetCubeSize()
        {
            return _cubeModel.CubeLenght;
        }
    }
}