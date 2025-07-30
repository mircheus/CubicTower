using UnityEngine;

namespace Game.Scripts.Events
{
    public interface IDestroyCubeEvents : IGlobalSubscriber
    {
        void OnCubeDestroyed(Vector3 position);
    }
}