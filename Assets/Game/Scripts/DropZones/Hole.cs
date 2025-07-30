using System;
using Game.Scripts.Cubes;
using UnityEngine;

namespace Game.Scripts.DropZones
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Hole : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        
        public Transform TargetTransform => targetTransform;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.TryGetComponent(out Cube cube))
            {
                if (cube.IsDragging == false)
                {
                    cube.SetVisibleInsideMask();
                }
            }
        }
    }
}