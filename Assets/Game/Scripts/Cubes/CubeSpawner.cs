using UnityEngine;
using Zenject;

namespace Game.Scripts.Cubes
{
    public class CubeSpawner
    {
        private readonly CubeFactory _cubeFactory;
        private readonly TowerManager _towerManager;
        private readonly Transform _poolParent;

        public CubeSpawner(CubeFactory cubeFactory, TowerManager towerManager, Transform poolParent)
        {
            _cubeFactory = cubeFactory;
            _towerManager = towerManager;
            _poolParent = poolParent;
        }
        
        public Cube SpawnCube(CubeType cubeType, Vector3 position)
        {
            if (cubeType == null)
            {
                Debug.LogError("CubeType is null. Cannot spawn cube.");
                return null;
            }
            
            var cube = _cubeFactory.GetCube(cubeType);
            cube.transform.position = position;
            cube.Destroyed += CubeOnDestroyed;
            // cube.transform.SetParent(_towerManager.transform);
            cube.OnSpawn();
            return cube;
        }

        private void CubeOnDestroyed(Cube cube)
        {
            cube.Destroyed -= CubeOnDestroyed;
            // cube.transform.SetParent(_poolParent);
            cube.OnDespawn();
           _cubeFactory.ReturnCubeToPool(cube);
        }
    }
}