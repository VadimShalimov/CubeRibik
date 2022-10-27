using UnityEngine;
using VContainer.Unity;

namespace Assets.Runtime.Controllers
{
    class CameraController: ITickable
    {
        private readonly Camera _camera;
        private readonly GameObject _cubeParent;

        public CameraController(Camera camera, GameObject cubeParent)
        {
            _cubeParent = cubeParent;
            _camera = camera;
        }
        public void Tick()
        {
            _camera.transform.LookAt(_cubeParent.transform);
        }
    }
}
