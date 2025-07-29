using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts
{
    public class TowerManager : MonoBehaviour
    {
        [SerializeField] private Transform floor;

        private List<Cube> _cubesList = new List<Cube>();
        
        public void AddCube(Cube cube)
        {
            if (cube == null)
            {
                Debug.LogError("Cubic is null. Cannot add to the list.");
                return;
            }
            
            if (_cubesList.Count <= 0)
            {
                AddCubeToList(cube);
                cube.SetFloor(floor);
                cube.MoveDownTo();
            }
            else
            {
                var targetCube = cube.GetTargetCube();

                if (_cubesList.Contains(targetCube) && _cubesList.IndexOf(targetCube) == _cubesList.Count - 1)
                {
                    if (cube.TryGetTargetPoint(out Vector2 targetPosition))
                    {
                        AddCubeToList(cube);
                        cube.MoveDownTo(targetPosition);
                    }
                }
                else
                {
                    Destroy(cube.gameObject);
                }
            }
        }

        private void AddCubeToList(Cube cube)
        {
            _cubesList.Add(cube);
            cube.DragStarted += OnDragStarted;
            cube.Destroyed += RemoveCubicFromList;
        }

        private void RemoveCubicFromList(Cube cube)
        {
            _cubesList.Remove(cube);
            cube.Destroyed -= RemoveCubicFromList;
            cube.DragStarted -= OnDragStarted;
        }

        private void OnDragStarted(Cube cube)
        {
            int cubicIndex = _cubesList.IndexOf(cube);
            
            for(int i = _cubesList.Count - 1; i > cubicIndex; i--)
            {
                _cubesList[i].MoveDownTo(_cubesList[i - 1].transform.position);
            }

            RemoveCubicFromList(cube);
        }
    }
}