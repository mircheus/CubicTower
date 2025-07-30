using System.Collections.Generic;
using Game.Scripts.Cubes;
using UnityEditor.Localization.Plugins.XLIFF.V12;
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
                Vector2 floorPosition = new Vector2(
                    cube.transform.position.x,
                    floor.position.y
                );
                
                PlaceCubeTo(floorPosition, cube);
                return;
            }
    
            TryPlaceCube(cube);
        }

        protected virtual void TryPlaceCube(Cube cube)
        {
            if (cube.TryGetTargetPoint(out RaycastHit2D targetHit))
            {
                var targetPoint = CalculateTargetPoint(targetHit);
                PlaceCubeTo(targetPoint, cube);
            }
            else
            {
                cube.SelfDestroy();
            }
        }
        
        protected virtual Vector3 CalculateTargetPoint(RaycastHit2D targetHit)
        {
            targetHit.collider.TryGetComponent(out Cube targetCube);
            var bounds = targetHit.collider.bounds;
            var halfSize = bounds.size / 2;
            var randomXOffset = RandomizeXPositonOffset(bounds.size);
            var targetPoint = new Vector2(
                targetCube.transform.position.x + randomXOffset,
                targetHit.point.y + halfSize.y
            );

            return targetPoint;
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
                _cubesList[i].SelfDestroy();
            }
            
            _cubesList.RemoveRange(index + 1, _cubesList.Count - index - 1);
        }

        private float RandomizeXPositonOffset(Vector3 boundsSize)
        {
            var halfSize = boundsSize / 2;
            float randomX = Random.Range(-halfSize.x, halfSize.x);
            return randomX;
        }
    }
}