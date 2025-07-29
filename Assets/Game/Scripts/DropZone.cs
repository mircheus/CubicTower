using Game.Scripts.Cubes;
using UnityEngine;

namespace Game.Scripts
{
    public class DropZone : MonoBehaviour
    {
        [SerializeField] private TowerManager towerManager;

        public void GetCubic(Cube cube)
        {
            towerManager.AddCube(cube);
        }
    }
}