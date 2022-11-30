using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Views
{
    public class UIPlaceholderView: MonoBehaviour
    {
        public event Action ShuffleButtonClicked; 
        public event Action AssembleButtonClicked; 

        [SerializeField] private Button _shuffleCubeButton;
        [SerializeField] private Button _assembleCubeButton;

        private void Start()
        {
            _shuffleCubeButton.onClick.AddListener(() => ShuffleButtonClicked?.Invoke());
            _assembleCubeButton.onClick.AddListener(() => AssembleButtonClicked?.Invoke());    
        }
    }
}