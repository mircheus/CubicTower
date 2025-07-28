using UnityEngine;

namespace Game.Scripts.DragAndDrop
{
    public interface IDraggable
    {
        void StartDrag(Transform floor);
        void EndDrag();
    }
}