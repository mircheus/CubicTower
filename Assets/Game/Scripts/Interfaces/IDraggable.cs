using UnityEngine;

namespace Game.Scripts.DragAndDrop
{
    public interface IDraggable
    {
        public bool IsDragging { get; }
        public bool IsAbleToDrag { get; }
        
        void StartDrag();
        void EndDrag();
    }
}