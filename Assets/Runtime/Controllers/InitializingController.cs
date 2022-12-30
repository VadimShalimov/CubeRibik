using System.Linq;
using System.Threading;

using Cysharp.Threading.Tasks;

using Runtime.Core;
using Runtime.Services;
using Runtime.Utils;
using VContainer.Unity;

namespace Runtime.Controllers
{
    public class InitializingController : IInitializable, IAsyncStartable
    {
        private readonly CubeModelFactory _cubeModelFactory;
        private readonly CubeViewFactory _viewFactory;
        private readonly CubeInteractionService _cubeInteractionService;

        public InitializingController(CubeModelFactory cubeModelFactory, CubeViewFactory viewFactory, CubeInteractionService cubeInteractionService)
        {
            _cubeModelFactory = cubeModelFactory;
            _viewFactory = viewFactory;
            _cubeInteractionService = cubeInteractionService;
        }

        void IInitializable.Initialize()
        {
            var cubeModel = _cubeModelFactory.CreateCubeModel();
            
            _cubeInteractionService.AddCubeModel(cubeModel);
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            var viewArray = await _viewFactory.CreateViewsAsync();
            _cubeInteractionService.AddCubeViews(viewArray.ToArray());
        }
    }
}