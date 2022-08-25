﻿using UnityEngine;
using UnityEngine.UI;

namespace Assets.Runtime.Views
{
    public class UiButtonsContainer: MonoBehaviour
    {
        public Button RandomButton => _randomButton;
        public Button ReverseButton => _reverseButton;

        [SerializeField] private Button _randomButton;
        [SerializeField] private Button _reverseButton;
    }
}
