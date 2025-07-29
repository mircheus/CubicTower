using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts
{
    public class TowerManager : MonoBehaviour
    {
        [SerializeField] private Transform floor;
        
        private List<Cubic> cubicList = new List<Cubic>();

        public void AddCubic(Cubic cubic)
        {
            if (cubic == null)
            {
                Debug.LogError("Cubic is null. Cannot add to the list.");
                return;
            }
            
            if (cubicList.Count <= 0)
            {
                cubicList.Add(cubic);
                cubic.SetFloor(floor);
                cubic.MoveDownTo();
            }
            else
            {
                if(cubic.TryGetTargetPoint(out Vector2 targetPosition))
                {
                    cubicList.Add(cubic);
                    cubic.MoveDownTo(targetPosition);
                }
                else
                {
                    Destroy(cubic.gameObject);
                }
            }
        }
    }
}