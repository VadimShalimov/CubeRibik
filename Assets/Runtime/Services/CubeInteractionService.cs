using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using Runtime.Enums;
using Runtime.Models;
using Runtime.Utils;
using Runtime.Utils.Extensions;

using UnityEngine;

using Vector2 = UnityEngine.Vector2;

namespace Runtime.Services
{
    public class CubeInteractionService
    {
        private CubeModel _cubeModel;

        private ModelRotationHelper _modelRotationHelper;

        private GameObject[] _gameObjects;

        private ViewRotationHelper _viewRotationHelper;

        public Vector2Int GetNestedSideCount()
        {
            return _cubeModel.NestedSideCount;
        }

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

        public async UniTask RotateCubeModelAsync(RotationValue rotationValue)
        {
            var cubeIndexes = _cubeModel.GetSideModel(rotationValue.RotationSide);

            await _viewRotationHelper.RotateSideAsync(GetCubes(cubeIndexes, rotationValue.Deep),
                rotationValue.RotationSide.GetRotateDirection(rotationValue.RotationCondition), rotationValue.Deep);
            Debug.Log(rotationValue.RotationSide);
            _modelRotationHelper.RotateCubeModel(rotationValue);
        }

        public Vector2 GetCubeSize()
        {
            return _cubeModel.CubeLenght;
        }

        private GameObject[] GetCubes(SideModel sideModel, int deep)
        {
            var cubeList = new List<GameObject>();

            var indexes = new List<int>();

            indexes.AddRange(sideModel.PanelModel.CubeIndexes);

            for (int i = 1; i < deep; i++)
            {
                indexes.AddRange(sideModel.AttachedPanelModels[i - 1].CubeIndexes);
            }

            foreach (var cubeIndex in indexes)
            {
                cubeList.Add(_gameObjects[cubeIndex]);
            }

            return cubeList.ToArray();
        }
    }
}