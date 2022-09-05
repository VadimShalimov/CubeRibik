using Assets.Runtime.Enumns;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using Assets.Runtime.Configs;
using Assets.Runtime.Views;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Assets.Runtime.Controllers
{
    public class CubeController : ITickable, IStartable, IDisposable, IAsyncStartable
    {
        private const int CentreOfCubeIndex = 13;
        private const int RotateAngleValue = 5;
        private const float _mouseSpeed = 4f;

        private readonly List<GameObject> _cubeList;
        private readonly GameObject _parentCube;
        private readonly UiButtonsContainer _buttons;
        private readonly int _numberForRandom;

        private List<GameObject> _upCubes => _cubeList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == 1);
        private List<GameObject> _downCubes => _cubeList.FindAll(x => Mathf.Round(x.transform.localPosition.y) == -1);
        private List<GameObject> _frontCubes => _cubeList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == 1);
        private List<GameObject> _backCubes => _cubeList.FindAll(x => Mathf.Round(x.transform.localPosition.x) == -1);
        private List<GameObject> _leftCubes => _cubeList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == 1);
        private List<GameObject> _rightCubes => _cubeList.FindAll(x => Mathf.Round(x.transform.localPosition.z) == -1);

        private List<CubeMoveTurns> _savedTurns = new List<CubeMoveTurns>();

        private bool _turnRecharge = true;

        private Vector2 _mouseInputValue;

        private Tween _rotateTween;

        private CancellationTokenSource _cts;
        private CancellationToken _cancellation;

        private Sequence _activeSequence;


        public CubeController(List<GameObject> cubeList, GameObject parentCube, CubeVisualConfig cubeVisual, UiButtonsContainer buttons)
        {
            _cubeList = cubeList;
            _parentCube = parentCube;
            _numberForRandom = cubeVisual.Turn;
            _buttons = buttons;
        }

        public void Start()
        {
            _buttons.RandomButton.onClick.AddListener(RandomTurnForButton);
            _buttons.ReverseButton.onClick.AddListener(CubeReverseButton);
            _buttons.StartRotate.onClick.AddListener(RotateWholeCube);
            _buttons.StopRotate.onClick.AddListener(StopRotateCube);
        }

        public void Dispose()
        {
            _buttons.RandomButton.onClick.RemoveListener(RandomTurnForButton);
            _buttons.ReverseButton.onClick.RemoveListener(CubeReverseButton);
            _buttons.StartRotate.onClick.RemoveListener(RotateWholeCube);
            _buttons.StopRotate.onClick.RemoveListener(StopRotateCube);
        }

        public UniTask StartAsync(CancellationToken cancellation)
        {
            _cancellation = cancellation;
            _cts = CancellationTokenSource.CreateLinkedTokenSource(_cancellation);

            return UniTask.CompletedTask;
        }

        public void Tick()
        {
            if (_turnRecharge)
                CheckInputForCube();

            InputMouseForRotate();
        }

        private void RotateWholeCube()
        {
            RotateCubeAsync(_cts.Token).Forget();
        }

        private void StopRotateCube()
        {
            DisposeToken();

            _cts = CancellationTokenSource.CreateLinkedTokenSource(_cancellation);
            _parentCube.transform.localScale = Vector3.one;
            _parentCube.transform.eulerAngles = Vector3.zero;
        }

        private async UniTaskVoid RotateCubeAsync(CancellationToken token)
        {
            _activeSequence = DOTween.Sequence();
            await _activeSequence.Append(_parentCube.transform.DORotate(new Vector3(90, 0, 0), 0.5f, RotateMode.LocalAxisAdd))
                .Append(_parentCube.transform.DORotate(new Vector3(0, 90, 0), 0.5f, RotateMode.LocalAxisAdd))
                .Append(_parentCube.transform.DORotate(new Vector3(0, 0, 90), 0.5f, RotateMode.LocalAxisAdd))
                .Join(_parentCube.transform.DOScale(Vector3.zero, 0.5f))
                .AwaitForComplete(cancellationToken: token);
        }

        private void DisposeToken()
        {
            if (_cts == null) return;

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        private void InputMouseForRotate()
        {
            _mouseInputValue.x += Input.GetAxis("Mouse X");
            _mouseInputValue.y += Input.GetAxis("Mouse Y");

            float xAxisRotation = Input.GetAxis("Mouse X") * _mouseSpeed;
            float yAxisRotation = Input.GetAxis("Mouse Y") * _mouseSpeed;

            
            if (Input.GetMouseButton(0))
            {

                //_parentCube.transform.Rotate(Vector3.down, xAxisRotation);
                //_parentCube.transform.Rotate(Vector3.right, yAxisRotation);
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
                int number = UnityEngine.Random.Range(0, 5);

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