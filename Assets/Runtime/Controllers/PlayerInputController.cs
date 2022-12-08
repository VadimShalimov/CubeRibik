using Runtime.Models;
using UnityEngine;
using VContainer.Unity;

namespace Runtime.Controllers
{
    public class PlayerInputController: ITickable
    {
        private readonly InputModel _inputModel;

        public PlayerInputController(InputModel inputModel)
        {
            _inputModel = inputModel;
        }

        void ITickable.Tick()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
               _inputModel.InvokeVerticalSideChange(-1); 
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                _inputModel.InvokeHorizontalSideChange(-1);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _inputModel.InvokeVerticalSideChange(1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                _inputModel.InvokeHorizontalSideChange(1);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                _inputModel.InvokeRotateAction();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _inputModel.InvokeRotateAction(true);
            }
        }
    }
}