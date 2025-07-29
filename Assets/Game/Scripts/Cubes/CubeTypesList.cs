using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Cubes
{
    [CreateAssetMenu(fileName = "CubesList", menuName = "Game/CubesList", order = 1)]
    public class CubeTypesList : ScriptableObject
    {
        [SerializeField] private List<CubeType> cubeTypes;
        
        public List<CubeType> CubeTypes => cubeTypes;
    }
}