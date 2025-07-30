using System;
using Game.Scripts.Cubes;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.DropZones
{
    public class DestroyDropZone : DropZone
    {
        [SerializeField] private LayerMask holeLayerMask;

        public override void GetCubic(Cube cube)
        {
            TryDropCube(cube);
        }

        private void TryDropCube(Cube cube)
        {
            var hit = cube.RaycastDown(holeLayerMask);
            
            if (hit.collider != null && hit.collider.TryGetComponent(out Hole hole))
            {
                EventBus.RaiseEvent<IMessageActionEvents>(message => message.Show(MessageAction.ThrowCube));
                cube.MoveDownTo(hole.TargetTransform.position, 1.5f);
            }
            else
            {
                cube.SelfDestroy();
            }
        }
    }
}