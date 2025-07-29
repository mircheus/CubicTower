using DG.Tweening;
using Game.Scripts.DragAndDrop;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts
{
    public class Cubic : MonoBehaviour, IDraggable
    {
        [SerializeField] private LayerMask cubicLayerMask;
        [SerializeField] private LayerMask dropZoneLayerMask;
        [SerializeField] private BoxCollider2D boxCollider;
        
        private Transform _floor;
        
        public void StartDrag(Transform floor)
        {
            _floor = floor;
        }

        public void EndDrag()
        {
            if (IsDropZone())
            {
                Raycast2DRay();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Raycast2DRay()
        { 
            var rayPoint = CalculateRayPoint();
            var result = Physics2D.Raycast(rayPoint, Vector2.down, 20f, cubicLayerMask); 
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

        private bool IsDropZone()
        {
            var colliders = Physics2D.OverlapBoxAll(
                boxCollider.bounds.center, 
                boxCollider.bounds.size, 
                0f, 
                dropZoneLayerMask
            );

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out DropZone dropZone))
                {
                    // Handle drop zone logic here
                    Debug.Log($"Dropped on: {collider.name}");
                    return true;
                }
            }

            Debug.Log("No valid drop zone found.");
            return false;
        }

        private Vector3 CalculateRayPoint()
        {
            var halfSize = boxCollider.bounds.size / 2;
            var position = transform.position;
            return new Vector3(position.x,
                position.y - halfSize.y - 0.1f, 
                position.z);
        }
    }
}