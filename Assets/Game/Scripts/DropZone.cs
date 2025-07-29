using UnityEngine;

namespace Game.Scripts
{
    public class DropZone : MonoBehaviour
    {
        [SerializeField] private TowerManager towerManager;

        public void GetCubic(Cubic cubic)
        {
            towerManager.AddCubic(cubic);
        }
    }
}