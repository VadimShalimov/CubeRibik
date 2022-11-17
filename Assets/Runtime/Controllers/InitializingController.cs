using Runtime.Core;

using VContainer.Unity;

namespace Runtime.Controllers
{
    public class InitializingController : IInitializable
    {
        private readonly CubeModelFactory _cubeModelFactory;
        private readonly CubeViewFactory _viewFactory;

        public InitializingController(CubeModelFactory cubeModelFactory, CubeViewFactory viewFactory)
        {
            _cubeModelFactory = cubeModelFactory;
            _viewFactory = viewFactory;
        }

        void IInitializable.Initialize()
        {
            var cubeModel = _cubeModelFactory.CreateCubeModel();
            var viewArray = _viewFactory.CreateViews();
        }
    }
}