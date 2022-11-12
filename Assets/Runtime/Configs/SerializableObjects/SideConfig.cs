using System;

using Editor.CustomAttributes;

using Runtime.Enums;

using UnityEngine;

namespace Assets.Runtime.Configs.SerializableObjects
{
    [Serializable]
    public class SideConfig
    {
        public Side CubeSide => _cubeSide;
        
        public CubeColors[] ColorsArray { get; private set; }
        
        [SerializeField] [ReadOnly] private Side _cubeSide;
        [SerializeField] [ReadOnly] private CubeColors _sideStartColor;

        [SerializeField] [Unity.Collections.ReadOnly] private CubeColors[] _colorsArray;

        public SideConfig(Side side, CubeColors cubeColor)
        {
            _cubeSide = side;
            _sideStartColor = cubeColor;
        }

        public void SetArrayLength(int xLength, int yLength)
        {
            ColorsArray = new CubeColors[xLength * yLength];

            for (int i = 0; i < ColorsArray.Length; i++)
            {
                ColorsArray[i] = _sideStartColor;
            }

            _colorsArray = ColorsArray;
        }
    }
}