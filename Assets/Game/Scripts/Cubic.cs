using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts
{
    public class Cubic : MonoBehaviour, IDraggable
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Transform rayPoint;
        
        private Transform _floor;

        public void StartDrag(Transform floor)
        {
            _floor = floor;
        }

        public void EndDrag()
        {
            MoveDown();
            Raycast2DRay();
        }

        private void Raycast2DRay()
        {
            var result = Physics2D.Raycast(rayPoint.position, Vector2.down, 20f, layerMask);
            
            if(result.collider != null)
            {
                var halfSize = result.collider.bounds.size / 2;
                var closestPoint = new Vector2(
                    result.point.x,
                    result.point.y + halfSize.y
                );
                MoveDown(closestPoint);
            }
            else
            {
                MoveDown();
            }
        }

        private void MoveDown(Vector3 position = default(Vector3))
        {
            Vector3 targetPosition = position != default(Vector3) ? position : _floor.position;
            transform.DOMoveY(targetPosition.y, 1f).SetEase(Ease.Linear);
        }
    }
}