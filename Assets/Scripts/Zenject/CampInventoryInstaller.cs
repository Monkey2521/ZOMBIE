using UnityEngine;

using ZombieSurvival.General.Inventories;

using Zenject;

public class CampInventoryInstaller : MonoInstaller
{
    [SerializeField] private CampInventory _campInventoryInstance;

    public override void InstallBindings()
    {
        Container.Bind<CampInventory>().FromInstance(_campInventoryInstance).AsSingle();
    }
}

