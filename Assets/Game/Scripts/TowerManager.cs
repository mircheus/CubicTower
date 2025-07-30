using System.Collections.Generic;
using Game.Scripts.Cubes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts
{
    public class TowerManager : MonoBehaviour
    {
        [Header("References: ")]
        [SerializeField] private Transform floor;
        
        [Header("Settings: ")]
        [SerializeField] private float cubeFallDuration = 1f;

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
                PlaceCubeTo(floor.position, cube);
                return;
            }
    
            TryPlaceCube(cube);
        }

        protected virtual void TryPlaceCube(Cube cube)
        {
            if (cube.TryGetTargetPoint(out Vector2 targetPosition))
            {
                PlaceCubeTo(targetPosition, cube);
            }
            else
            {
                cube.SelfDestroy();
            }
        }
        
        private void PlaceCubeTo(Vector2 position, Cube cube)
        {
            if (cube == null)
            {
                Debug.LogError("Cube is null. Cannot place it.");
                return;
            }
            
            AddCubeToList(cube);
            cube.MoveDownTo(position, cubeFallDuration);
            
            DebugLogCubesList();
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
            
            DebugLogCubesList();
        }

        private void OnDragStarted(Cube cube)
        {
            int cubeIndex = _cubesList.IndexOf(cube);

            if (IsCubesFallAvailable(cubeIndex))
            {
                for(int i = _cubesList.Count - 1; i > cubeIndex; i--)
                {
                    _cubesList[i].MoveDownTo(_cubesList[i - 1].transform.position, cubeFallDuration);
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
                Destroy(_cubesList[i].gameObject); // TODO: self-destroy
            }
            
            _cubesList.RemoveRange(index + 1, _cubesList.Count - index - 1);
        }
        
        private void DebugLogCubesList()
        {
            // Debug.Log("__________________");
            // for(int i = _cubesList.Count - 1; i >= 0; i--)
            // {
            //     Debug.Log($"{i}. {_cubesList[i].CubeType}");
            // }
        }
    }
}