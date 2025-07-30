using System;
using DG.Tweening;
using Game.Scripts.CubeParticleSystem;
using Game.Scripts.DragAndDrop;
using Game.Scripts.DropZones;
using Game.Scripts.Events;
using Game.Scripts.ObjectPool;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace Game.Scripts.Cubes
{
    public class Cube : MonoBehaviour, IDraggable, IPoolable, IDestroyable
    {
        [Header("References: ")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private LayerMask cubeLayerMask;
        [SerializeField] private LayerMask dropZoneLayerMask;
        [SerializeField] private BoxCollider2D boxCollider;
        
        private Transform _floor;
        private CubeType _cubeType;
        private bool _isDragging;

        public event Action<Cube> DragStarted;
        public event Action<Cube> Destroyed;
        
        public CubeType CubeType => _cubeType;
        public bool IsDragging => _isDragging;

        public void Initialize(CubeType cubeType)
        {
            _cubeType = cubeType;
            spriteRenderer.sprite = _cubeType.CubeSprite;
        }
        
        public void StartDrag()
        {
            _isDragging = true;
            DragStarted?.Invoke(this);
        }

        public void EndDrag()
        {
            _isDragging = false;
            
            if (IsOverlapCubes())
            {
                SelfDestroy();
            }
            
            if (IsDropZone()) // TODO: переписать 
            {
                // Raycast2DRay();
            }
            else
            {
                SelfDestroy();
            }
        }

        public void MoveDownTo(Vector2 position, float duration)
        {
            transform.DOMoveY(position.y, duration).SetEase(Ease.InQuint);
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

        public CubeType GetBeneathCubeColor()
        {
            var result = RaycastDown();

            if (result.collider != null && result.collider.TryGetComponent(out Cube cube))
            {
                return cube.CubeType;
            }

            return null;
        }
        
        public bool IsAnyCubeBeneath()
        {
            var rayPoint = CalculateRayPoint();
            var hits = Physics2D.RaycastAll(rayPoint, Vector2.down, boxCollider.bounds.size.y + 0.1f, cubeLayerMask);
            return hits.Length > 1;
        }

        public void OnSpawn()
        {
            gameObject.SetActive(true);
        }

        public void OnDespawn()
        {
            gameObject.SetActive(false);
        }

        public void SelfDestroy()
        {
            Destroyed?.Invoke(this);
            EventBus.RaiseEvent<IDestroyCubeEvents>(module => module.OnCubeDestroyed(transform.position));
        }

        public void SetVisibleInsideMask()
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        public RaycastHit2D RaycastDown(LayerMask layerMask = default)
        {
            var rayPoint = CalculateRayPoint();
            var raycastMask = layerMask == default ? cubeLayerMask : layerMask;
            return Physics2D.Raycast(rayPoint, Vector2.down, 40f, raycastMask);
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

        private bool IsOverlapCubes()
        {
            var colliders = Physics2D.OverlapBoxAll(
                boxCollider.bounds.center, 
                boxCollider.bounds.size, 
                0f, 
                cubeLayerMask
            );
            
            return colliders.Length > 1;
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