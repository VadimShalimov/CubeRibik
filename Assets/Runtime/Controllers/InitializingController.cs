using Assets.Runtime.Configs;
using Runtime.Core;
using Runtime.Utils;
using VContainer.Unity;

namespace Runtime.Controllers
{
    public class InitializingController : IInitializable
    {
        private readonly CubeModelFactory _cubeModelFactory;
        private readonly CubeViewFactory _viewFactory;
        private readonly CubeRepositoryService _cubeRepositoryService;

        public InitializingController(CubeModelFactory cubeModelFactory, CubeViewFactory viewFactory, CubeRepositoryService cubeRepositoryService)
        {
            _cubeModelFactory = cubeModelFactory;
            _viewFactory = viewFactory;
            _cubeRepositoryService = cubeRepositoryService;
        }

        void IInitializable.Initialize()
        {
            var cubeModel = _cubeModelFactory.CreateCubeModel();
            var viewArray = _viewFactory.CreateViews();
            _cubeRepositoryService.AddCubeModel(cubeModel);
        }
    }
}