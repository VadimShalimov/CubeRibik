using Assets.Runtime.Enumns;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using Assets.Runtime.Configs;
using Assets.Runtime.Views;

namespace Assets.Runtime.Controllers
{
    public class CubeController : ITickable, IStartable
    {
        private const int CentreOfCubeIndex = 13;
        private const int RotateAngleValue = 5;
        private const float _mouseSpeed = 4f;

        private readonly List<GameObject> _cubeList;
        private readonly GameObject _parentCube;
        private readonly UiButtonsContainer _buttons;
        private readonly int _numberForRandom;
        private readonly CubeView _cubeView;
        private List<GameObject> _upCubes => _cubeList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == 1);
        private List<GameObject> _downCubes => _cubeList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -1);
        private List<GameObject> _frontCubes => _cubeList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == 1);
        private List<GameObject> _backCubes => _cubeList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -1);
        private List<GameObject> _leftCubes => _cubeList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 1);
        private List<GameObject> _rightCubes => _cubeList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 1);

        private List<CubeMoveTurns> _savedTurns = new List<CubeMoveTurns>();

        private bool _turnRecharge = true;

        private Vector2 _mouseInputValue;

        public void Start()
        {
            _buttons.RandomButton.onClick.AddListener(RandomTurnForButton);
            _buttons.ReverseButton.onClick.AddListener(CubeReverseButton);
        }

        public CubeController(List<GameObject> cubeList, GameObject parentCube, CubeVisualConfig cubeVisual, UiButtonsContainer buttons)
        {
            _cubeList = cubeList;
            _parentCube = parentCube;
            _numberForRandom = cubeVisual.Turn;
            _buttons = buttons;
        }

        public void Tick()
        {
            if (_turnRecharge)
                CheckInputForCube();

            InputMouseForRotate();
        }

        private void InputMouseForRotate()
        {
            _mouseInputValue.x += Input.GetAxis("Mouse X");
            _mouseInputValue.y += Input.GetAxis("Mouse Y");

            float xAxisRotation = Input.GetAxis("Mouse X") * _mouseSpeed;
            float yAxisRotation = Input.GetAxis("Mouse Y") * _mouseSpeed;

            
            if (Input.GetMouseButton(0))
            {

                _parentCube.transform.Rotate(Vector3.down, xAxisRotation);
                _parentCube.transform.Rotate(Vector3.right, yAxisRotation);
                //   _parentCube.transform.localRotation = Quaternion.Euler(-_mouseInputValue.y, _mouseInputValue.x, 0);
            }
        }

        private void CheckInputForCube()
        {
            if (Input.GetKeyDown(KeyCode.W))
                RotateCube(_upCubes, CubeMoveTurns.Up, true);
            else if (Input.GetKeyDown(KeyCode.S))
                RotateCube(_downCubes, CubeMoveTurns.Down, true);
            else if (Input.GetKeyDown(KeyCode.A))
                RotateCube(_leftCubes, CubeMoveTurns.Left, true);
            else if (Input.GetKeyDown(KeyCode.D))
                RotateCube(_rightCubes, CubeMoveTurns.Right, true);
            else if (Input.GetKeyDown(KeyCode.Q))
                RotateCube(_frontCubes, CubeMoveTurns.Front, true);
            else if (Input.GetKeyDown(KeyCode.E))
                RotateCube(_backCubes, CubeMoveTurns.Back, true);
        }

        private void RandomTurnForButton()
        {
            for (int count = _numberForRandom; count >= 1; count--)
            {
                int number = Random.Range(0, 5);

                switch (number)
                {
                    case 0: RotateCube(_upCubes, CubeMoveTurns.Up, true);
                        break;
                    case 1: RotateCube(_downCubes, CubeMoveTurns.Down, true);
                        break;
                    case 2: RotateCube(_leftCubes, CubeMoveTurns.Left, true);
                        break;
                    case 3: RotateCube(_rightCubes, CubeMoveTurns.Right, true);
                        break;
                    case 4: RotateCube(_frontCubes, CubeMoveTurns.Front, true);
                        break;
                    case 5: RotateCube(_backCubes, CubeMoveTurns.Back, true);
                        break;
                }
            }
        }

        public void CubeReverseButton()
        {
            _savedTurns.Reverse();

            foreach (var savedTurn in _savedTurns)
            {
                switch (savedTurn)
                {
                    case CubeMoveTurns.Up:
                        RotateCube(_upCubes, CubeMoveTurns.AntiUp, false);
                        continue;
                    case CubeMoveTurns.Down:
                        RotateCube(_downCubes, CubeMoveTurns.AntiDown, false);
                        continue;
                    case CubeMoveTurns.Left:
                        RotateCube(_leftCubes, CubeMoveTurns.AntiLeft, false);
                        continue;
                    case CubeMoveTurns.Right:
                        RotateCube(_rightCubes, CubeMoveTurns.AntiRight, false);
                        continue;
                    case CubeMoveTurns.Back:
                        RotateCube(_backCubes, CubeMoveTurns.AntiBack, false);
                        continue;
                    case CubeMoveTurns.Front:
                        RotateCube(_frontCubes, CubeMoveTurns.AntiFront, false);
                        continue;
                }
            }

            _savedTurns.Clear();
        }

        private void RotateCube(List<GameObject> cubeListSide, CubeMoveTurns cubeMoveTurn, bool savedTurn)
        {
            _turnRecharge = false;
            int angle = 0;

            while (angle < 90)
            {
                foreach (GameObject cube in cubeListSide)
                    cube.transform.RotateAround(_cubeList[CentreOfCubeIndex].transform.position, cubeMoveTurn.MoveTurnToVector(), RotateAngleValue);
                angle += RotateAngleValue;
            }

            if (savedTurn)
            {
                _savedTurns.Add(cubeMoveTurn);
            }

            _turnRecharge = true;
        }
    }
}