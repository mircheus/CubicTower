namespace Game.Scripts.ObjectPool
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}