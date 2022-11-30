using System;

namespace Runtime.Models
{
    public class InputModel
    {
        public event Action<int> HorizontalSideChanged;
        public event Action<int> VerticalSideChanged;
        public event Action<bool> RotateAction;

        public void InvokeHorizontalSideChange(int value)
        {
            HorizontalSideChanged?.Invoke(value);
        }
        
        public void InvokeVerticalSideChange(int value)
        {
            VerticalSideChanged?.Invoke(value);
        }

        public void InvokeRotateAction(bool defaultRotateState = false)
        {
            RotateAction?.Invoke(defaultRotateState);
        }
    }
}