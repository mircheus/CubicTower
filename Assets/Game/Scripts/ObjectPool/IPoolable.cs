using UnityEngine;

namespace Game.Scripts.Cubes
{
    public interface IPoolable
    {
        void OnSpawn(Vector2 position);
        void OnDespawn();
    }
}