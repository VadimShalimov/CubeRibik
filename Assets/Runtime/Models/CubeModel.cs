using System;
using System.Linq;

using Runtime.Enums;
using UnityEngine;

namespace Runtime.Models
{
    public class CubeModel
    {
        public Vector2Int CubeLenght => _cubeLenght;
        
        private readonly SideModel[] _sidesData;
        private Vector2Int _cubeLenght;
        
        public CubeModel(SideModel[] sidesData, Vector2Int cubeLenght)
        {
            _sidesData = sidesData;
            _cubeLenght = cubeLenght;

            ConfigureSidesGraphs();
        }
        
        public SideModel GetSideModel(Side side)
        {
            return _sidesData.First(x => x.Side.Equals(side));
        }

        private void ConfigureSidesGraphs()
        {
            foreach (var sideModel in _sidesData)
            {
                switch (sideModel.Side)
                {
                    case Side.UpSide:
                        sideModel.SetConnectedSideModels(GetSideModel(Side.FrontSide),GetSideModel(Side.RightSide), 
                            GetSideModel(Side.BackSide), GetSideModel(Side.LeftSide));
                        break;
                    case Side.DownSide:
                        sideModel.SetConnectedSideModels(GetSideModel(Side.FrontSide),GetSideModel(Side.RightSide),
                            GetSideModel(Side.BackSide), GetSideModel(Side.LeftSide));
                        break;
                    case Side.LeftSide:
                        sideModel.SetConnectedSideModels(GetSideModel(Side.UpSide),GetSideModel(Side.FrontSide),
                            GetSideModel(Side.DownSide), GetSideModel(Side.BackSide));
                        break;
                    case Side.RightSide:
                        sideModel.SetConnectedSideModels(GetSideModel(Side.UpSide),GetSideModel(Side.FrontSide),
                            GetSideModel(Side.DownSide), GetSideModel(Side.BackSide));
                        break;
                    case Side.FrontSide:
                        sideModel.SetConnectedSideModels(GetSideModel(Side.UpSide),GetSideModel(Side.RightSide),
                            GetSideModel(Side.DownSide), GetSideModel(Side.LeftSide));
                        break;
                    case Side.BackSide:
                        sideModel.SetConnectedSideModels(GetSideModel(Side.UpSide),GetSideModel(Side.RightSide),
                            GetSideModel(Side.DownSide), GetSideModel(Side.LeftSide));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}