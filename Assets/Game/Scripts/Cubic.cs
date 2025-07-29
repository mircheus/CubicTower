using System;
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
        private float _cubicHeight;
        
        public event Action<Cubic> DragStarted;
        public event Action DragEnded;
        public event Action<Cubic> Destroyed;

        public float CubicHeight => _cubicHeight;
        
        private void OnEnable()
        {
            _cubicHeight = boxCollider.bounds.size.y;
        }

        public void StartDrag()
        {
            DragStarted?.Invoke(this);
        }

        public void EndDrag()
        {
            if (IsDropZone()) // TODO: переписать 
            {
                // Raycast2DRay();
            }
            else
            {
                SelfDestroy();
            }
        }

        private void SelfDestroy() // TODO: переделать на pool
        {
            Destroyed?.Invoke(this);
            Destroy(gameObject);
        }

        public void SetFloor(Transform floor)
        {
            _floor = floor;
        }

        public void MoveDownTo(Vector3 position = default(Vector3))
        {
            Vector3 targetPosition = position != default(Vector3) ? position : _floor.position;
            transform.DOMoveY(targetPosition.y, 1f).SetEase(Ease.Linear);
        }

        public bool TryGetTargetPoint(out Vector2 targetPoint)
        { 
            var rayPoint = CalculateRayPoint();
            var result = Physics2D.Raycast(rayPoint, Vector2.down, 40f, cubicLayerMask);
            
            if(result.collider != null)
            {
                var halfSize = result.collider.bounds.size / 2;
                targetPoint = new Vector2(
                    result.point.x,
                    result.point.y + halfSize.y
                );

                return true;
            }

            targetPoint = Vector2.zero;
            return false;
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
                    dropZone.GetCubic(this);
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