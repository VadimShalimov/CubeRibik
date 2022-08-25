using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer.Unity;

namespace Assets.Runtime.Controllers
{
    class CameraController: ITickable
    {
        private readonly GameObject _cubeParent;
        private readonly Camera _camera;

        public CameraController(GameObject cubeParent, Camera camera)
        {
            _cubeParent = cubeParent;
            _camera = camera;
        }
        public void Tick()
        {
            //_camera.transform.LookAt(_cubeParent.transform);
        }
    }
}
