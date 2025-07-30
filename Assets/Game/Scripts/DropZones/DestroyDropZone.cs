using System;
using Game.Scripts.Cubes;
using UnityEngine;

namespace Game.Scripts.DropZones
{
    public class DestroyDropZone : DropZone
    {
        [SerializeField] private Hole hole;
        
        public override void GetCubic(Cube cube)
        {
            cube.MoveDownTo(hole.TargetTransform.position, 1.5f);
        }
    }
}