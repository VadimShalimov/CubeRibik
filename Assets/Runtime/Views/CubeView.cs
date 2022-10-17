using Assets.Runtime.Enumns;
using UnityEngine;

namespace Assets.Runtime.Views
{
    public class CubeView : MonoBehaviour
    {
        public ParsingSide[] Sides => _sidesArray;

        private ParsingSide[] _sidesArray = new ParsingSide[3];
   
        public void SetSides(params ParsingSide[] parsingSide)
        {
            _sidesArray = parsingSide;
        }
    }
}