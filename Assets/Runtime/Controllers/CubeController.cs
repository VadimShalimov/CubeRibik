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
using System.Linq;

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

            _cubeList[0].SetSides(ParsingSide.LeftSide, ParsingSide.BackSide, ParsingSide.UpSide);
            _cubeList[1].SetSides(ParsingSide.BackSide, ParsingSide.UpSide);
            _cubeList[2].SetSides(ParsingSide.BackSide, ParsingSide.RightSide, ParsingSide.UpSide);
            _cubeList[3].SetSides(ParsingSide.BackSide, ParsingSide.LeftSide);
            _cubeList[4].SetSides(ParsingSide.BackSide);
            _cubeList[5].SetSides(ParsingSide.BackSide, ParsingSide.RightSide);
            _cubeList[6].SetSides(ParsingSide.BackSide, ParsingSide.LeftSide, ParsingSide.DownSide);
            _cubeList[7].SetSides(ParsingSide.BackSide, ParsingSide.DownSide);
            _cubeList[8].SetSides(ParsingSide.BackSide, ParsingSide.DownSide, ParsingSide.RightSide);


            _cubeList[18].SetSides(ParsingSide.FrontSide, ParsingSide.LeftSide, ParsingSide.UpSide);
            _cubeList[19].SetSides(ParsingSide.FrontSide, ParsingSide.UpSide);
            _cubeList[20].SetSides(ParsingSide.FrontSide, ParsingSide.RightSide, ParsingSide.UpSide);
            _cubeList[21].SetSides(ParsingSide.FrontSide, ParsingSide.LeftSide);
            _cubeList[22].SetSides(ParsingSide.FrontSide);
            _cubeList[23].SetSides(ParsingSide.FrontSide, ParsingSide.RightSide);
            _cubeList[24].SetSides(ParsingSide.FrontSide, ParsingSide.LeftSide, ParsingSide.DownSide);
            _cubeList[25].SetSides(ParsingSide.FrontSide, ParsingSide.DownSide);
            _cubeList[26].SetSides(ParsingSide.FrontSide, ParsingSide.RightSide, ParsingSide.DownSide);

            _cubeList[9].SetSides(ParsingSide.LeftSide, ParsingSide.UpSide);
            _cubeList[12].SetSides(ParsingSide.LeftSide);
            _cubeList[15].SetSides(ParsingSide.LeftSide, ParsingSide.DownSide);

            _cubeList[10].SetSides(ParsingSide.UpSide);
            _cubeList[14].SetSides(ParsingSide.RightSide);
            _cubeList[17].SetSides(ParsingSide.RightSide, ParsingSide.DownSide);
            _cubeList[11].SetSides(ParsingSide.RightSide, ParsingSide.UpSide);

            _cubeList[16].SetSides(ParsingSide.DownSide);
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
                RotateSide(ParsingForAllSide(ParsingSide.UpSide), CubeMoveTurns.UpForSide, true).Forget();
                UpdateSides(ParsingSide.UpSide, ParsingSide.FrontSide, ParsingSide.LeftSide, ParsingSide.RightSide, ParsingSide.BackSide);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                RotateSide(ParsingForAllSide(ParsingSide.DownSide), CubeMoveTurns.DownForSide, true).Forget();
                UpdateSides(ParsingSide.DownSide, ParsingSide.BackSide, ParsingSide.RightSide, ParsingSide.LeftSide, ParsingSide.FrontSide);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                RotateSide(ParsingForAllSide(ParsingSide.LeftSide), CubeMoveTurns.LeftForSide, true).Forget();
                UpdateSides(ParsingSide.LeftSide, ParsingSide.UpSide, ParsingSide.FrontSide, ParsingSide.BackSide, ParsingSide.DownSide);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RotateSide(ParsingForAllSide(ParsingSide.RightSide), CubeMoveTurns.RightForSide, true).Forget();
                UpdateSides(ParsingSide.RightSide, ParsingSide.UpSide, ParsingSide.BackSide, ParsingSide.FrontSide, ParsingSide.DownSide);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateSide(ParsingForAllSide(ParsingSide.FrontSide), CubeMoveTurns.FrontForSide, true).Forget();
                UpdateSides(ParsingSide.FrontSide, ParsingSide.DownSide, ParsingSide.RightSide, ParsingSide.LeftSide, ParsingSide.UpSide);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RotateSide(ParsingForAllSide(ParsingSide.BackSide), CubeMoveTurns.BackForSide, true).Forget();
                UpdateSides(ParsingSide.BackSide, ParsingSide.UpSide, ParsingSide.LeftSide, ParsingSide.RightSide, ParsingSide.DownSide);
            }
        }

        private void UpdateSides(ParsingSide sideOne, ParsingSide sideTwo, ParsingSide sideTree, ParsingSide sideFour, ParsingSide sideFive)
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
                        RotateSide(ParsingForAllSide(ParsingSide.UpSide), CubeMoveTurns.UpForSide, true);
                        break;
                    case 1:
                        RotateSide(ParsingForAllSide(ParsingSide.DownSide), CubeMoveTurns.DownForSide, true);
                        break;
                    case 2:
                        RotateSide(ParsingForAllSide(ParsingSide.LeftSide), CubeMoveTurns.LeftForSide, true);
                        break;
                    case 3:
                        RotateSide(ParsingForAllSide(ParsingSide.RightSide), CubeMoveTurns.RightForSide, true);
                        break;
                    case 4:
                        RotateSide(ParsingForAllSide(ParsingSide.FrontSide), CubeMoveTurns.FrontForSide, true);
                        break;
                    case 5:
                        RotateSide(ParsingForAllSide(ParsingSide.BackSide), CubeMoveTurns.BackForSide, true);
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
                        RotateSide(ParsingForAllSide(ParsingSide.UpSide), CubeMoveTurns.AntiUpForSide, false);
                        continue;
                    case CubeMoveTurns.DownForSide:
                        RotateSide(ParsingForAllSide(ParsingSide.DownSide), CubeMoveTurns.AntiDownForSide, false);
                        continue;
                    case CubeMoveTurns.LeftForSide:
                        RotateSide(ParsingForAllSide(ParsingSide.LeftSide), CubeMoveTurns.AntiLeftForSide, false);
                        continue;
                    case CubeMoveTurns.RightForSide:
                        RotateSide(ParsingForAllSide(ParsingSide.RightSide), CubeMoveTurns.AntiRightForSide, false);
                        continue;
                    case CubeMoveTurns.BackForSide:
                        RotateSide(ParsingForAllSide(ParsingSide.BackSide), CubeMoveTurns.AntiBackForSide, false);
                        continue;
                    case CubeMoveTurns.FrontForSide:
                        RotateSide(ParsingForAllSide(ParsingSide.FrontSide), CubeMoveTurns.AntiFrontForSide, false);
                        continue;
                }
            }

            _savedTurns.Clear();
        }

        private async UniTaskVoid RotateSide(List<CubeView> cubeListSide, CubeMoveTurns cubeMoveTurn, bool savedTurn)
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
            ****
            GameObject.Destroy(objectVoid);

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

        public List<CubeView> ParsingForAllSide(ParsingSide parsingSide)
        {
            switch (parsingSide)
            {
                case ParsingSide.UpSide:
                    return _cubeList.Where(x => x.Sides.Contains(ParsingSide.UpSide)).ToList();
                case ParsingSide.DownSide:
                    return _cubeList.Where(x => x.Sides.Contains(ParsingSide.DownSide)).ToList();
                case ParsingSide.RightSide:
                    return _cubeList.Where(x => x.Sides.Contains(ParsingSide.RightSide)).ToList();
                case ParsingSide.LeftSide:
                    return _cubeList.Where(x => x.Sides.Contains(ParsingSide.LeftSide)).ToList();
                case ParsingSide.FrontSide:
                    return _cubeList.Where(x => x.Sides.Contains(ParsingSide.FrontSide)).ToList();
                case ParsingSide.BackSide:
                    return _cubeList.Where(x => x.Sides.Contains(ParsingSide.BackSide)).ToList();
            }
            return null;
        }
    }
}