using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using Game.Scripts.Cubes;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("References: ")]
    [SerializeField] private Cube cubePrefab; 
    [SerializeField] private CubeTypesList cubeTypesList;
    [SerializeField] private Transform poolParent;
    
    [Header("Settings: ")]
    [SerializeField] private int poolSize = 25; // TODO: вынести в настройки
    
    public override void InstallBindings()
    {
        Container.Bind<CubeTypesList>().FromInstance(cubeTypesList).AsSingle();
        BindCubeFactory();
    }
    
    private void BindCubeFactory()
    {
        ObjectPool<Cube> objectPool = new ObjectPool<Cube>(cubePrefab, poolSize, poolParent);
        CubeFactory cubeFactory = new CubeFactory(cubeTypesList, objectPool);
        Container.Bind<CubeFactory>().FromInstance(cubeFactory).AsSingle().NonLazy();
    }
}
