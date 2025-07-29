using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts
{
    public class TowerManager : MonoBehaviour
    {
        [SerializeField] private Transform floor;
        
        private List<Cubic> _cubicList = new List<Cubic>();
        
        public void AddCubic(Cubic cubic)
        {
            if (cubic == null)
            {
                Debug.LogError("Cubic is null. Cannot add to the list.");
                return;
            }
            
            if (_cubicList.Count <= 0)
            {
                AddCubicToList(cubic);
                cubic.SetFloor(floor);
                cubic.MoveDownTo();
            }
            else
            {
                if(cubic.TryGetTargetPoint(out Vector2 targetPosition))
                {
                    AddCubicToList(cubic);
                    cubic.MoveDownTo(targetPosition);
                }
                else
                {
                    Destroy(cubic.gameObject);
                }
            }
        }

        private void AddCubicToList(Cubic cubic)
        {
            _cubicList.Add(cubic);
            cubic.DragStarted += OnDragStarted;
            cubic.Destroyed += RemoveCubicFromList;
        }

        private void RemoveCubicFromList(Cubic cubic)
        {
            _cubicList.Remove(cubic);
            cubic.Destroyed -= RemoveCubicFromList;
            cubic.DragStarted -= OnDragStarted;
        }

        private void OnDragStarted(Cubic cubic)
        {
            int cubicIndex = _cubicList.IndexOf(cubic);
            
            for(int i = _cubicList.Count - 1; i > cubicIndex; i--)
            {
                _cubicList[i].MoveDownTo(_cubicList[i - 1].transform.position);
            }

            RemoveCubicFromList(cubic);
        }
    }
}