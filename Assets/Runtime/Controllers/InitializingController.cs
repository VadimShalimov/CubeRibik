using System.Threading;

using Cysharp.Threading.Tasks;

using Runtime.Core;
using Runtime.Utils;
using VContainer.Unity;

namespace Runtime.Controllers
{
    public class InitializingController : IInitializable, IAsyncStartable
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
            
            _cubeRepositoryService.AddCubeModel(cubeModel);
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            var viewArray = await _viewFactory.CreateViewsAsync();
        }
    }
}