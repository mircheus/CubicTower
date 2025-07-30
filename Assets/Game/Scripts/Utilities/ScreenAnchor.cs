using UnityEngine;
using Zenject;

namespace Game.Scripts
{
    public class ScreenAnchor : MonoBehaviour
    {
        [SerializeField] private Vector2 anchor = new(0.5f, 0.5f);
        private Camera _camera;

        [Inject]
        public void Construct(Camera mainCamera)
        {
            _camera = mainCamera;
        }
        
        private void OnEnable()
        {
            SetPosition();
        }
        
        private void SetPosition()
        {
            var v3Pos = new Vector3(anchor.x, anchor.y, 0.25f);
            var v3Center = new Vector3(0.5f, 0.5f, 0.25f);
            if (!_camera)
            {
                _camera = Camera.main;
            }

            if (_camera != null)
            {
                Vector3 pos = _camera.ViewportToWorldPoint(v3Pos);
                Vector3 posCenter = _camera.ViewportToWorldPoint(v3Center);
                var dif = pos - posCenter;
                
                var trf = transform;
                pos.z = trf.position.z;
                trf.position = new Vector3(0,0,0) + dif;
            }
        }
    }
}