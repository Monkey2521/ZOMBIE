using UnityEngine;

using Zenject;
using ZombieSurvival.General.Inventories;

public class LevelsInventoryInstaller : MonoInstaller
{
    [SerializeField] private LevelInventory _levelsInventoryInstance;

    public override void InstallBindings()
    {
        Container.Bind<LevelInventory>().FromInstance(_levelsInventoryInstance).AsSingle();
    }
}