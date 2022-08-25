using UnityEngine;
using Assets.Runtime.Configs;
using VContainer.Unity;
using System.Collections.Generic;
using Assets.Runtime.Controllers;
using Assets.Runtime.Views;

namespace Assets.Runtime.Core
{
    public class StartUp : IStartable
    {
        private readonly LifetimeScope _lifetimeScope;
        private readonly CubeVisualConfig _visualConfig;
        private readonly GameObject _cubeParent;

        public StartUp(LifetimeScope lifetimeScope, CubeVisualConfig visualConfig, GameObject cubeParent)
        {
            _lifetimeScope = lifetimeScope;
            _visualConfig = visualConfig;
            _cubeParent = cubeParent;
        }

        public void Start()
        {
            var cubeList = new List<GameObject>();

            for (int x = -1; x < 2; x++)
                for (int y = -1; y < 2; y++)
                    for (int z = -1; z < 2; z++)
                    {
                        var cube = Object.Instantiate(_visualConfig.CubePrefab, _cubeParent.transform);
                        cube.transform.localPosition = new Vector3(-x, -y, z);
                        cubeList.Add(cube);
                    }

            _lifetimeScope.CreateChild(builder =>
            {
                builder.RegisterEntryPoint<CameraController>();
                builder.RegisterComponent(cubeList);
                builder.RegisterEntryPoint<CubeController>();
            });
        }
    }
}