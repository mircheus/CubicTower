
using Game.Scripts.Cubes;
using UnityEngine;

namespace Game.Scripts.DropZones
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DestroyTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out IDestroyable destroyable))
            {
                destroyable.SelfDestroy();
            }
        }
    }
}