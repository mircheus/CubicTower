using UnityEngine;

namespace Game.Scripts
{
    public interface IDraggable
    {
        void StartDrag(Transform floor);
        void EndDrag();
    }
}