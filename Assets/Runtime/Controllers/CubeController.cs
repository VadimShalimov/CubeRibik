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

using Object = UnityEngine.Object;

namespace Assets.Runtime.Controllers
{
    public class CubeController : ITickable, IStartable, IDisposable, IAsyncStartable
    {
        private const int CentreOfCubeIndex = 13;
        private const float _mouseSpeed = 4f;

        private readonly List<CubeView> _cubeList;
        private readonly GameObject _parentCube;
        private readonly UiButtonsContainer _buttons;
        private readonly int _numberForRandom;

        private List<CubeMoveTurns> _savedTurns = new List<CubeMoveTurns>();

        private bool _turnRecharge = true;

        private Vector2 _mouseInputValue;

        private CancellationTokenSource _cts;
        private CancellationToken _cancellation;

        private Sequence _activeSequence;

        public CubeController(List<CubeView> cubeList, GameObject parentCube, CubeVisualConfig cubeVisual, UiButtonsContainer buttons)
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
                RotateSideAsync(GetTargetCubesList(CubeSide.UpSide), CubeMoveTurns.UpForSide, true).Forget();
                UpdateSides(CubeSide.UpSide, CubeSide.FrontSide, CubeSide.LeftSide, CubeSide.RightSide, CubeSide.BackSide);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                RotateSideAsync(GetTargetCubesList(CubeSide.DownSide), CubeMoveTurns.DownForSide, true).Forget();
                UpdateSides(CubeSide.DownSide, CubeSide.BackSide, CubeSide.RightSide, CubeSide.LeftSide, CubeSide.FrontSide);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                RotateSideAsync(GetTargetCubesList(CubeSide.LeftSide), CubeMoveTurns.LeftForSide, true).Forget();
                UpdateSides(CubeSide.LeftSide, CubeSide.UpSide, CubeSide.FrontSide, CubeSide.BackSide, CubeSide.DownSide);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RotateSideAsync(GetTargetCubesList(CubeSide.RightSide), CubeMoveTurns.RightForSide, true).Forget();
                UpdateSides(CubeSide.RightSide, CubeSide.UpSide, CubeSide.BackSide, CubeSide.FrontSide, CubeSide.DownSide);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateSideAsync(GetTargetCubesList(CubeSide.FrontSide), CubeMoveTurns.FrontForSide, true).Forget();
                UpdateSides(CubeSide.FrontSide, CubeSide.DownSide, CubeSide.RightSide, CubeSide.LeftSide, CubeSide.UpSide);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RotateSideAsync(GetTargetCubesList(CubeSide.BackSide), CubeMoveTurns.BackForSide, true).Forget();
                UpdateSides(CubeSide.BackSide, CubeSide.UpSide, CubeSide.LeftSide, CubeSide.RightSide, CubeSide.DownSide);
            }
        }

        private void UpdateSides(CubeSide sideOne, CubeSide sideTwo, CubeSide sideTree, CubeSide sideFour, CubeSide sideFive)
        {
            var leftUp = _cubeList.Where(x => x.Sides.Contains(sideOne) && x.Sides.Contains(sideTwo) && x.Sides.Contains(sideTree)).ToList();//7
            var rightUp = _cubeList.Where(x => x.Sides.Contains(sideOne) && x.Sides.Contains(sideTwo) && x.Sides.Contains(sideFour)).ToList();//9
            var leftDown = _cubeList.Where(x => x.Sides.Contains(sideOne) && x.Sides.Contains(sideFive) && x.Sides.Contains(sideTree)).ToList();//1
            var rightdDown = _cubeList.Where(x => x.Sides.Contains(sideOne) && x.Sides.Contains(sideFive) && x.Sides.Contains(sideFour)).ToList();//3

            var left = _cubeList.Where(x => x.Sides.Contains(sideOne) && x.Sides.Contains(sideTree)).ToList();
            var right = _cubeList.Where(x => x.Sides.Contains(sideOne) && x.Sides.Contains(sideFour)).ToList();
            var down = _cubeList.Where(x => x.Sides.Contains(sideOne) && x.Sides.Contains(sideFive)).ToList();
            var up = _cubeList.Where(x => x.Sides.Contains(sideOne) && x.Sides.Contains(sideTwo)).ToList();

            for (int i = 0; i < 27; i++)
            {
                var cube = _cubeList[i];

                if (leftUp.Contains(cube))
                {
                    _cubeList[i].SetSides(sideOne, sideTwo, sideFour);
                }

                if (rightUp.Contains(cube))
                {
                    _cubeList[i].SetSides(sideOne, sideFive, sideFour);
                }

                if (leftDown.Contains(cube))
                {
                    _cubeList[i].SetSides(sideOne, sideTwo, sideTree);
                }
                
                if (rightdDown.Contains(cube))
                {
                    _cubeList[i].SetSides(sideOne, sideFive, sideTree);
                }

                if (left.Contains(cube))
                {
                    _cubeList[i].SetSides(sideOne, sideTwo);
                }

                if (right.Contains(cube))
                {
                    _cubeList[i].SetSides(sideOne, sideFive);
                }

                if (down.Contains(cube))
                {
                    _cubeList[i].SetSides(sideOne, sideTree);
                }

                if (up.Contains(cube))
                {
                    _cubeList[i].SetSides(sideOne, sideFour);
                }
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
                        RotateSideAsync(GetTargetCubesList(CubeSide.UpSide), CubeMoveTurns.UpForSide, true);
                        break;
                    case 1:
                        RotateSideAsync(GetTargetCubesList(CubeSide.DownSide), CubeMoveTurns.DownForSide, true);
                        break;
                    case 2:
                        RotateSideAsync(GetTargetCubesList(CubeSide.LeftSide), CubeMoveTurns.LeftForSide, true);
                        break;
                    case 3:
                        RotateSideAsync(GetTargetCubesList(CubeSide.RightSide), CubeMoveTurns.RightForSide, true);
                        break;
                    case 4:
                        RotateSideAsync(GetTargetCubesList(CubeSide.FrontSide), CubeMoveTurns.FrontForSide, true);
                        break;
                    case 5:
                        RotateSideAsync(GetTargetCubesList(CubeSide.BackSide), CubeMoveTurns.BackForSide, true);
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
                    case CubeMoveTurns.UpForSide:
                        RotateSideAsync(GetTargetCubesList(CubeSide.UpSide), CubeMoveTurns.AntiUpForSide, false);
                        continue;
                    case CubeMoveTurns.DownForSide:
                        RotateSideAsync(GetTargetCubesList(CubeSide.DownSide), CubeMoveTurns.AntiDownForSide, false);
                        continue;
                    case CubeMoveTurns.LeftForSide:
                        RotateSideAsync(GetTargetCubesList(CubeSide.LeftSide), CubeMoveTurns.AntiLeftForSide, false);
                        continue;
                    case CubeMoveTurns.RightForSide:
                        RotateSideAsync(GetTargetCubesList(CubeSide.RightSide), CubeMoveTurns.AntiRightForSide, false);
                        continue;
                    case CubeMoveTurns.BackForSide:
                        RotateSideAsync(GetTargetCubesList(CubeSide.BackSide), CubeMoveTurns.AntiBackForSide, false);
                        continue;
                    case CubeMoveTurns.FrontForSide:
                        RotateSideAsync(GetTargetCubesList(CubeSide.FrontSide), CubeMoveTurns.AntiFrontForSide, false);
                        continue;
                }
            }

            _savedTurns.Clear();
        }

        private async UniTaskVoid RotateSideAsync(List<CubeView> cubeListSide, CubeMoveTurns cubeMoveTurn, bool savedTurn)
        {
            _turnRecharge = false;

            var objectVoid = new GameObject();

            objectVoid.transform.localPosition = _cubeList[CentreOfCubeIndex].gameObject.transform.localPosition;

            foreach (var cube in cubeListSide)
            {
                cube.gameObject.transform.SetParent(objectVoid.transform);
            }

            await objectVoid.transform.DOLocalRotate(cubeMoveTurn.MoveTurnToVector(), 0.5f, RotateMode.Fast);

            if (savedTurn)
            {
                _savedTurns.Add(cubeMoveTurn);
            }

            foreach (var cubeInParent in cubeListSide)
            {
                cubeInParent.gameObject.transform.SetParent(_parentCube.transform);
            }

            foreach (var cube in cubeListSide)
            {
                
            }
            
            Object.Destroy(objectVoid);

            _turnRecharge = true;
        }

        private async UniTaskVoid RotateCube(List<CubeView> cubeListSide, CubeMoveTurns cubeMoveTurn, bool savedTurn)
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