using System;

namespace Runtime.Models
{
    public class InputModel
    {
        public bool AvailableToInput { get; private set; }
        
        public event Action<int> HorizontalSideChanged;
        public event Action<int> VerticalSideChanged;
        public event Action<bool> RotateAction;

        public void EnableInputFilter()
        {
            AvailableToInput = true;
        }

        public void DisableInputFilter()
        {
            AvailableToInput = false;
        }

        public void InvokeHorizontalSideChange(int value)
        {
            InvokeActionIfPermitted(HorizontalSideChanged, value);
        }
        
        public void InvokeVerticalSideChange(int value)
        {
            InvokeActionIfPermitted(VerticalSideChanged,value);
        }

        public void InvokeRotateAction(bool defaultRotateState = false)
        {
            InvokeActionIfPermitted(RotateAction, defaultRotateState);
        }

        private void InvokeActionIfPermitted<T>(Action<T> action,T genericValue)
        {
            if (!AvailableToInput)
            {
                return;
            }
            
            action?.Invoke(genericValue);
        }
    }
}