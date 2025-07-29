using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Cubes;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private CubeTypesList cubeTypesList;
    
    public override void InstallBindings()
    {
        Container.Bind<CubeTypesList>().FromInstance(cubeTypesList).AsSingle();
    }
}
