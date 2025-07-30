using Game.Scripts.Cubes;
using UnityEngine;

namespace Game.Scripts.DropZones
{
    public class TowerDropZone : DropZone
    {
        [SerializeField] private TowerManager towerManager;

        public override void GetCubic(Cube cube)
        {
            towerManager.AddCube(cube);
        }
    }
}