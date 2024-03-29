using Assets.Runtime.Configs;

using Runtime.Controllers;
using Runtime.Core;
using Runtime.Models;
using Runtime.Services;
using Runtime.Views;
using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace Runtime.Root
{
    public class InitializingLifetimeScope : LifetimeScope
    {
        [SerializeField] private CubeModelGameplayConfig _cubeModelGameplayConfig;

        [SerializeField] private CubeVisualConfig _cubeVisualConfig;

        [SerializeField] private UIPlaceholderView _uiPlaceholderView;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<CubeModelFactory>(Lifetime.Singleton).WithParameter(_cubeModelGameplayConfig);

            builder.Register<CubeViewFactory>(Lifetime.Singleton).WithParameter(_cubeModelGameplayConfig)
                .WithParameter(_cubeVisualConfig);

            builder.RegisterComponent(_uiPlaceholderView);

            builder.Register<InputModel>(Lifetime.Singleton);
            builder.Register<CubeInteractionService>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<InitializingController>();

            builder.RegisterEntryPoint<PlayerInputController>();

            builder.RegisterEntryPoint<CubeInputPresenter>();

            builder.RegisterEntryPoint<BlendController>();
        }
    }
}