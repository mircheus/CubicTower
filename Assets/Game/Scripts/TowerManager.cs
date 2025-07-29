using System.Collections.Generic;
using Game.Scripts.Cubes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts
{
    public class TowerManager : MonoBehaviour
    {
        [SerializeField] private Transform floor;

        private List<Cube> _cubesList = new List<Cube>();
        
        public virtual void AddCube(Cube cube)
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
            int cubeIndex = _cubesList.IndexOf(cube);

            if (IsCubesFallAvailable(cubeIndex))
            {
                for(int i = _cubesList.Count - 1; i > cubeIndex; i--)
                {
                    _cubesList[i].MoveDownTo(_cubesList[i - 1].transform.position);
                }
            }
            else
            {
                DestroyAllCubesAbove(cubeIndex);
            }
            
            RemoveCubicFromList(cube);
        }

        private bool IsCubesFallAvailable(int index)
        {
            if (index == _cubesList.Count - 1)
            {
                return true;
            }

            return _cubesList[index + 1].IsAnyCubeBeneath();
        }

        private void DestroyAllCubesAbove(int index)
        {
            for (int i = _cubesList.Count - 1; i > index; i--)
            {
                Destroy(_cubesList[i].gameObject);
            }
            
            _cubesList.RemoveRange(index + 1, _cubesList.Count - index - 1);
        }
    }
}