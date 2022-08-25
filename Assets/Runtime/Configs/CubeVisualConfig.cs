using UnityEngine;

namespace Assets.Runtime.Configs
{
    [CreateAssetMenu(fileName = nameof(CubeVisualConfig),
        menuName = "Configs/Cube" + nameof(CubeVisualConfig), order = 0)]
    public class CubeVisualConfig : ScriptableObject
    {
        public int Turn => _turn;
        public GameObject CubePrefab => _cubePrefab;

        [SerializeField] private int _turn;
        [SerializeField] private GameObject _cubePrefab;
    }
}
