using Assets.Runtime.Configs.SerializableObjects;

using EasyButtons;

using Runtime.Enums;

using UnityEditor;

using UnityEngine;

namespace Assets.Runtime.Configs
{
    [CreateAssetMenu(fileName = nameof(CubeModelGameplayConfig), menuName = "MENUNAME", order = 0)]
    public class CubeModelGameplayConfig : ScriptableObject
    {
        public SideConfig[] SideConfigs => _sidesConfigs;
        
        public Vector2Int CubeSizeVector => new Vector2Int(_xzDimensionalLength, _yDimensionalLength);
        
        [SerializeField] [Unity.Collections.ReadOnly] private SideConfig[] _sidesConfigs;
        
        [SerializeField] private int _xzDimensionalLength = 3;

        [SerializeField] private int _yDimensionalLength = 3;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_xzDimensionalLength < 3)
            {
                _xzDimensionalLength = 3;
            }

            if (_yDimensionalLength < 1)
            {
                _yDimensionalLength = 1;
            }
            
            if (_sidesConfigs.Length == 0)
            {
                _sidesConfigs = GenerateStartConfig();
            }
            
            if (_sidesConfigs != null)
            {
                _sidesConfigs[0].SetArrayLength(_xzDimensionalLength, _xzDimensionalLength);

                for (var index = 1; index < _sidesConfigs.Length - 1; index++)
                {
                    var side = _sidesConfigs[index];
                    side.SetArrayLength(_xzDimensionalLength, _yDimensionalLength);
                }
                
                _sidesConfigs[5].SetArrayLength(_xzDimensionalLength, _xzDimensionalLength);
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

        [Button]
        private void RunGame()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.EnterPlaymode();
            }
        }
#endif
    }
}