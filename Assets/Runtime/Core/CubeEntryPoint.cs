using Assets.Runtime.Core;
using Assets.Runtime.Configs;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Assets.Runtime.Views;

public class CubeEntryPoint : LifetimeScope
{
    [SerializeField] private CubeVisualConfig _cubeConfig;
    [SerializeField] private GameObject _cubeParent;
    [SerializeField] private UiButtonsContainer _buttons;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<StartUp>();
        builder.RegisterInstance(_cubeConfig);
        builder.RegisterComponent(_cubeParent);
        builder.RegisterComponent(_buttons);
        builder.RegisterComponentInHierarchy<Camera>();
    }
}
