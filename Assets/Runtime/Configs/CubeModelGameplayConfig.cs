using Assets.Runtime.Configs.SerializableObjects;

using Runtime.Enums;

using Unity.Collections;

using UnityEngine;

namespace Assets.Runtime.Configs
{
    [CreateAssetMenu(fileName = nameof(CubeModelGameplayConfig), menuName = "MENUNAME", order = 0)]
    public class CubeModelGameplayConfig : ScriptableObject
    {
        [SerializeField] [ReadOnly] private SideConfig[] _sidesConfigs;
        
        [SerializeField] private int _sideLenght;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_sidesConfigs.Length == 0)
            {
                _sidesConfigs = GenerateStartConfig();
            }
            
            if (_sidesConfigs != null)
            {
                foreach (var side in _sidesConfigs)
                {
                    side.SetArrayLength(_sideLenght);
                }
            }
        }

        private SideConfig[] GenerateStartConfig()
        {
            return new[]
            {
                new SideConfig(Side.UpSide, CubeColors.White),
                new SideConfig(Side.LeftSide, CubeColors.Blue),
                new SideConfig(Side.RightSide, CubeColors.Green),
                new SideConfig(Side.FrontSide, CubeColors.Red),
                new SideConfig(Side.BackSide, CubeColors.Orange),
                new SideConfig(Side.DownSide, CubeColors.Yellow),
            };
        }
#endif
    }
}