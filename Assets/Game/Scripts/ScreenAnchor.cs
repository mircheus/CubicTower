using UnityEngine;

namespace Game.Scripts
{
    public class ScreenAnchor : MonoBehaviour
    {
        [SerializeField] private Vector2 anchor = new(0.5f, 0.5f);
        [SerializeField] private Camera camera;

        private void OnEnable()
        {
            SetPosition();
        }
        
        private void SetPosition()
        {
            var v3Pos = new Vector3(anchor.x, anchor.y, 0.25f);
            var v3Center = new Vector3(0.5f, 0.5f, 0.25f);
            if (!camera)
            {
                camera = Camera.main;
            }

            if (camera != null)
            {
                Vector3 pos = camera.ViewportToWorldPoint(v3Pos);
                Vector3 posCenter = camera.ViewportToWorldPoint(v3Center);
                var dif = pos - posCenter;
                
                var trf = transform;
                pos.z = trf.position.z;
                trf.position = new Vector3(0,0,0) + dif;
            }
        }
    }
}