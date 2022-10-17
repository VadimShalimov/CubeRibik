using Assets.Runtime.Enums;
using UnityEngine;

namespace Assets.Runtime.Views
{
    public class CubeView : MonoBehaviour
    {
        public CubeSide[] Sides => _sidesArray;

        private CubeSide[] _sidesArray = new CubeSide[3];
   
        public void SetSides(params CubeSide[] parsingSide)
        {
            _sidesArray = parsingSide;
        }
    }
}