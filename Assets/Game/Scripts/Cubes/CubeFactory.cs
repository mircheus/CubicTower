using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Cubes
{
    public class CubeFactory
    {
        private List<CubeType> _cubeTypes;
        private ObjectPool<Cube> _cubePool;
        
        public CubeFactory(CubeTypesList cubeTypesList, ObjectPool<Cube> cubePool)
        {
            _cubeTypes = new List<CubeType>(cubeTypesList.CubeTypes);
            _cubePool = cubePool;
        }
        
        public Cube GetCube(CubeType cubeType)
        {
            if (_cubeTypes.Contains(cubeType) == false)
            {
                Debug.LogError($"FigureType - {cubeType} is not presented in Dictionary.");
            }
            
            var cube = _cubePool.Get();
            cube.Initialize(cubeType);
            return cube;
        }
        
        public void ReturnCubeToPool(Cube figure)
        {
            _cubePool.ReturnToPool(figure);
        }
    }
}