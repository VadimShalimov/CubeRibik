using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Views
{
    public class UIPlaceholderView : MonoBehaviour
    {
        public event Action ShuffleButtenClicked;
        public event Action AssembleButtenClicked;

        [SerializeField] private Button _shuffleCubeButton;
        [SerializeField] private Button _assembleCubeButton;

        private void Start()
        {
            _shuffleCubeButton.onClick.AddListener(() => ShuffleButtenClicked?.Invoke());
            _assembleCubeButton.onClick.AddListener(() => AssembleButtenClicked?.Invoke());
            
            
        }
    }
}