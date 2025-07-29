using UnityEngine;
using Zenject;

namespace Game.Scripts.Cubes
{
    public class CubeSpawner
    {
        private CubeFactory _cubeFactory;
        
        public CubeSpawner(CubeFactory cubeFactory)
        {
            _cubeFactory = cubeFactory;
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
            cube.OnSpawn();
            return cube;
        }

        private void CubeOnDestroyed(Cube cube)
        {
            cube.Destroyed -= CubeOnDestroyed;
            cube.OnDespawn();
           _cubeFactory.ReturnCubeToPool(cube);
        }
    }
}