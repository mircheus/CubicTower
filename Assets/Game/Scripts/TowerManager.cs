using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class TowerManager : MonoBehaviour
    {
        [SerializeField] private Transform floor;
        
        private List<Cubic> cubicList = new List<Cubic>();
        
        public void AddCubic(Cubic cubic)
        {
            if (cubic != null && !cubicList.Contains(cubic))
            {
                cubicList.Add(cubic);
                cubic.SetFloor(floor);
                Debug.Log("Cubic added: " + cubic.name);
            }
            else
            {
                Debug.LogWarning("Cubic is null or already exists in the list.");
            }
        }
    }
}