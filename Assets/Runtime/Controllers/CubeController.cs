using Assets.Runtime.Enums;

using System.Collections.Generic;

using UnityEngine;

using VContainer.Unity;

using Assets.Runtime.Configs;
using Assets.Runtime.Views;

using DG.Tweening;

using Cysharp.Threading.Tasks;

using System.Threading;
using System;
using System.Linq;

namespace Assets.Runtime.Controllers
{
    public class CubeController : ITickable, IStartable, IDisposable, IAsyncStartable
    {
        private const float _mouseSpeed = 4f;

        private readonly List<CubeView> _cubeList;
        private readonly GameObject _parentCube;
        private readonly LifetimeScope _scope;
        private readonly UiButtonsContainer _buttons;
        private readonly int _numberForRandom;
        
        private GameObject _objectJoin;

        private List<CubeMoveTurns> _savedTurns = new List<CubeMoveTurns>();

        private bool _turnRecharge = true;

        private Vector2 _mouseInputValue;

        private CancellationTokenSource _cts;
        private CancellationToken _cancellation;

        private Sequence _activeSequence;

        public CubeController(List<CubeView> cubeList, GameObject parentCube, CubeVisualConfig cubeVisual, UiButtonsContainer buttons, LifetimeScope scope)
        {
            _cubeList = cubeList;
            _parentCube = parentCube;
            _scope = scope;
            _numberForRandom = cubeVisual.Turn;
            _buttons = buttons;
        }

        public void Start()
        {
            _objectJoin = new GameObject();
            
            _objectJoin.transform.SetParent(_scope.transform);
            
            _buttons.RandomButton.onClick.AddListener(RandomTurnForButton);
            _buttons.ReverseButton.onClick.AddListener(CubeReverseButton);
            _buttons.StartRotate.onClick.AddListener(RotateWholeCube);
            _buttons.StopRotate.onClick.AddListener(StopRotateCube);

            _cubeList[0].SetSides(CubeSide.LeftSide, CubeSide.BackSide, CubeSide.UpSide);
            _cubeList[1].SetSides(CubeSide.BackSide, CubeSide.UpSide);
            _cubeList[2].SetSides(CubeSide.BackSide, CubeSide.RightSide, CubeSide.UpSide);
            _cubeList[3].SetSides(CubeSide.BackSide, CubeSide.LeftSide);
            _cubeList[4].SetSides(CubeSide.BackSide);
            _cubeList[5].SetSides(CubeSide.BackSide, CubeSide.RightSide);
            _cubeList[6].SetSides(CubeSide.BackSide, CubeSide.LeftSide, CubeSide.DownSide);
            _cubeList[7].SetSides(CubeSide.BackSide, CubeSide.DownSide);
            _cubeList[8].SetSides(CubeSide.BackSide, CubeSide.DownSide, CubeSide.RightSide);


            _cubeList[18].SetSides(CubeSide.FrontSide, CubeSide.LeftSide, CubeSide.UpSide);
            _cubeList[19].SetSides(CubeSide.FrontSide, CubeSide.UpSide);
            _cubeList[20].SetSides(CubeSide.FrontSide, CubeSide.RightSide, CubeSide.UpSide);
            _cubeList[21].SetSides(CubeSide.FrontSide, CubeSide.LeftSide);
            _cubeList[22].SetSides(CubeSide.FrontSide);
            _cubeList[23].SetSides(CubeSide.FrontSide, CubeSide.RightSide);
            _cubeList[24].SetSides(CubeSide.FrontSide, CubeSide.LeftSide, CubeSide.DownSide);
            _cubeList[25].SetSides(CubeSide.FrontSide, CubeSide.DownSide);
            _cubeList[26].SetSides(CubeSide.FrontSide, CubeSide.RightSide, CubeSide.DownSide);

            _cubeList[9].SetSides(CubeSide.LeftSide, CubeSide.UpSide);
            _cubeList[12].SetSides(CubeSide.LeftSide);
            _cubeList[15].SetSides(CubeSide.LeftSide, CubeSide.DownSide);

            _cubeList[10].SetSides(CubeSide.UpSide);
            _cubeList[14].SetSides(CubeSide.RightSide);
            _cubeList[17].SetSides(CubeSide.RightSide, CubeSide.DownSide);
            _cubeList[11].SetSides(CubeSide.RightSide, CubeSide.UpSide);

            _cubeList[16].SetSides(CubeSide.DownSide);
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
            
            //InputMouseForRotate();
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

                _parentCube.transform.Rotate(Vector3.down, xAxisRotation);
                _parentCube.transform.Rotate(Vector3.right, yAxisRotation);
                _parentCube.transform.localRotation = Quaternion.Euler(-_mouseInputValue.y, _mouseInputValue.x, 0);
            }
        }

        private void CheckInputForCube()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                RotateSideAsync(GetTargetCubesList(CubeSide.UpSide), CubeMoveTurns.CornerUp, true).Forget();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                RotateSideAsync(GetTargetCubesList(CubeSide.DownSide), CubeMoveTurns.CornerDown, true).Forget();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                RotateSideAsync(GetTargetCubesList(CubeSide.LeftSide), CubeMoveTurns.CornerLeft, true).Forget();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RotateSideAsync(GetTargetCubesList(CubeSide.RightSide), CubeMoveTurns.CornerRight, true).Forget();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateSideAsync(GetTargetCubesList(CubeSide.FrontSide), CubeMoveTurns.CornerFront, true).Forget();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RotateSideAsync(GetTargetCubesList(CubeSide.BackSide), CubeMoveTurns.CornerBack, true).Forget();
            }
        }

        private void RandomTurnForButton()
        {

            for (int count = _numberForRandom; count >= 1; count--)
            {
                int number = UnityEngine.Random.Range(0, 5);

                switch (number)
                {
                    case 0:
                        RotateSideAsync(GetTargetCubesList(CubeSide.UpSide), CubeMoveTurns.CornerUp, true).Forget();
                        break;
                    case 1:
                        RotateSideAsync(GetTargetCubesList(CubeSide.DownSide), CubeMoveTurns.CornerDown, true).Forget();
                        break;
                    case 2:
                        RotateSideAsync(GetTargetCubesList(CubeSide.LeftSide), CubeMoveTurns.CornerLeft, true).Forget();
                        break;
                    case 3:
                        RotateSideAsync(GetTargetCubesList(CubeSide.RightSide), CubeMoveTurns.CornerRight, true).Forget();
                        break;
                    case 4:
                        RotateSideAsync(GetTargetCubesList(CubeSide.FrontSide), CubeMoveTurns.CornerFront, true).Forget();
                        break;
                    case 5:
                        RotateSideAsync(GetTargetCubesList(CubeSide.BackSide), CubeMoveTurns.CornerBack, true).Forget();
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
                    case CubeMoveTurns.CornerUp:
                        RotateSideAsync(GetTargetCubesList(CubeSide.UpSide), CubeMoveTurns.AntiCornerUp, false).Forget();
                        continue;
                    case CubeMoveTurns.CornerDown:
                        RotateSideAsync(GetTargetCubesList(CubeSide.DownSide), CubeMoveTurns.AntiCornerDown, false).Forget();
                        continue;
                    case CubeMoveTurns.CornerLeft:
                        RotateSideAsync(GetTargetCubesList(CubeSide.LeftSide), CubeMoveTurns.AntiCornerLeft, false).Forget();
                        continue;
                    case CubeMoveTurns.CornerRight:
                        RotateSideAsync(GetTargetCubesList(CubeSide.RightSide), CubeMoveTurns.AntiCornerRight, false).Forget();
                        continue;
                    case CubeMoveTurns.CornerBack:
                        RotateSideAsync(GetTargetCubesList(CubeSide.BackSide), CubeMoveTurns.AntiCornerBack, false).Forget();
                        continue;
                    case CubeMoveTurns.CornerFront:
                        RotateSideAsync(GetTargetCubesList(CubeSide.FrontSide), CubeMoveTurns.AntiCornerFront, false).Forget();
                        continue;
                }
            }

            _savedTurns.Clear();
        }

        private async UniTaskVoid RotateSideAsync(List<CubeView> cubeListSide, CubeMoveTurns cubeMoveTurn, bool savedTurn)
        {
            _turnRecharge = false;

            foreach (var cube in cubeListSide)
            {
                cube.gameObject.transform.SetParent(_objectJoin.transform);
            }
            
            if (savedTurn)
            {
                _savedTurns.Add(cubeMoveTurn);
            }

            await _objectJoin.transform.DOLocalRotate(cubeMoveTurn.MoveTurnToVector(), 0.5f, RotateMode.Fast);
            
            foreach (var cubeInParent in cubeListSide)
            {
                cubeInParent.gameObject.transform.SetParent(_parentCube.transform);
            }

            _objectJoin.transform.localEulerAngles = Vector3.zero;

            _turnRecharge = true;
        }

        private async UniTaskVoid RotateCubeAsync(List<CubeView> cubeListSide, CubeMoveTurns cubeMoveTurn, bool savedTurn)
        {
            _turnRecharge = false;

            foreach (var cube in cubeListSide)
                await cube.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);

            if (savedTurn)
            {
                _savedTurns.Add(cubeMoveTurn);
            }

            _turnRecharge = true;
        }

        private List<CubeView> GetTargetCubesList(CubeSide cubeSide)
        {
            switch (cubeSide)
            {
                case CubeSide.UpSide:
                    return _cubeList.Where(x => x.Sides.Contains(CubeSide.UpSide)).ToList();
                case CubeSide.DownSide:
                    return _cubeList.Where(x => x.Sides.Contains(CubeSide.DownSide)).ToList();
                case CubeSide.RightSide:
                    return _cubeList.Where(x => x.Sides.Contains(CubeSide.RightSide)).ToList();
                case CubeSide.LeftSide:
                    return _cubeList.Where(x => x.Sides.Contains(CubeSide.LeftSide)).ToList();
                case CubeSide.FrontSide:
                    return _cubeList.Where(x => x.Sides.Contains(CubeSide.FrontSide)).ToList();
                case CubeSide.BackSide:
                    return _cubeList.Where(x => x.Sides.Contains(CubeSide.BackSide)).ToList();
            }

            return null;
        }
    }
}