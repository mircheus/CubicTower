using Game.Scripts.Cubes;
using Game.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Game.Scripts
{
    public class GameInstaller : MonoInstaller
    {
        [Header("References: ")]
        [SerializeField] private Cube cubePrefab; 
        [SerializeField] private CubeTypesList cubeTypesList;
        [SerializeField] private Transform poolParent;
        [SerializeField] private TowerManager towerManager;
        [SerializeField] private MessagesDictionary messagesDictionary;
    
        [Header("Settings: ")]
        [SerializeField] private int poolSize = 25; // TODO: вынести в настройки
    
        public override void InstallBindings()
        {
            Container.Bind<CubeTypesList>().FromInstance(cubeTypesList).AsSingle();
            Container.Bind<MessagesDictionary>().FromInstance(messagesDictionary).AsSingle();
            BindSpawner();
        }

        private void BindSpawner()
        {
            ObjectPool<Cube> objectPool = new ObjectPool<Cube>(cubePrefab, poolSize, poolParent);
            CubeFactory cubeFactory = new CubeFactory(cubeTypesList, objectPool);
            CubeSpawner cubeSpawner = new CubeSpawner(cubeFactory, towerManager, poolParent);
            Container.Bind<CubeFactory>().FromInstance(cubeFactory).AsSingle().NonLazy();
            Container.Bind<CubeSpawner>().FromInstance(cubeSpawner).AsSingle().NonLazy();
        }
    }
}
