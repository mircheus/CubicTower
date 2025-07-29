using Game.Scripts.Cubes;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI
{
    public class CubesScrollView : MonoBehaviour
    {
        [SerializeField] private GameObject content;
        [SerializeField] private CubeIcon cubeIconPrefab;
        
        private CubeTypesList _cubeTypesList;
        
        [Inject]
        public void Construct(CubeTypesList cubeTypesList)
        {
            _cubeTypesList = cubeTypesList;
        }
        
        private void Start()
        {
            foreach (var cubeType in _cubeTypesList.CubeTypes)
            {
                var cubeItem = Instantiate(cubeIconPrefab, content.transform);
                cubeItem.Initialize(cubeType);
            }
        }
    }
}