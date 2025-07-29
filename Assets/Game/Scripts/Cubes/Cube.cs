using System;
using DG.Tweening;
using Game.Scripts.Cubes;
using Game.Scripts.DragAndDrop;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts
{
    public class Cube : MonoBehaviour, IDraggable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private LayerMask cubeLayerMask;
        [SerializeField] private LayerMask dropZoneLayerMask;
        [SerializeField] private BoxCollider2D boxCollider;
        
        private Transform _floor;
        private CubeType _cubeType;

        public event Action<Cube> DragStarted;
        public event Action<Cube> Destroyed;

        public void Initialize(CubeType cubeType)
        {
            _cubeType = cubeType;
            spriteRenderer.sprite = _cubeType.CubeSprite;
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
            var result = RaycastDown();
            
            if(result.collider != null && result.collider.TryGetComponent(out Cube cube))
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

        public virtual Cube GetTargetCube()
        {
            var result = RaycastDown();

            if (result.collider != null && result.collider.TryGetComponent(out Cube cube))
            {
                return cube;
            }

            return null;
        }

        public bool IsAnyCubeBeneath()
        {
            var rayPoint = CalculateRayPoint();
            var hits = Physics2D.RaycastAll(rayPoint, Vector2.down, cubeLayerMask);
            return hits.Length > 1;
        }

        public RaycastHit2D RaycastDown()
        {
            var rayPoint = CalculateRayPoint();
            return Physics2D.Raycast(rayPoint, Vector2.down, 40f, cubeLayerMask);
        }

        private void SelfDestroy() // TODO: переделать на pool
        {
            Destroyed?.Invoke(this);
            Destroy(gameObject);
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