using Game.Scripts.Cubes;
using UnityEngine;

namespace Game.Scripts.DropZones
{
    public abstract class DropZone : MonoBehaviour
    {
        public abstract void GetCubic(Cube cube);
    }
}