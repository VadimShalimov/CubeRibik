using Assets.Runtime.Configs;

using Runtime.Controllers;
using Runtime.Core;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace Runtime.Root
{
    public class InitializingLifetimeScope : LifetimeScope
    {
        [SerializeField] private CubeModelGameplayConfig _cubeModelGameplayConfig;

        [SerializeField] private CubeVisualConfig _cubeVisualConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<CubeModelFactory>(Lifetime.Singleton).WithParameter(_cubeModelGameplayConfig);

            builder.Register<CubeViewFactory>(Lifetime.Singleton).WithParameter(_cubeModelGameplayConfig)
                .WithParameter(_cubeVisualConfig);

            builder.RegisterEntryPoint<InitializingController>();
        }
    }
}