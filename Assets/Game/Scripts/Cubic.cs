using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts
{
    public class Cubic : MonoBehaviour, IDraggable
    {
        private Transform _floor;

        public void StartDrag(Transform floor)
        {
            _floor = floor;
        }

        public void EndDrag()
        {
            transform.DOMoveY(_floor.position.y, 1f).SetEase(Ease.Linear);
        }
    }
}