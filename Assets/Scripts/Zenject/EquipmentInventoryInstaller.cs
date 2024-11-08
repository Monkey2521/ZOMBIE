using UnityEngine;

using ZombieSurvival.General.Inventories;

using Zenject;

public class EquipmentInventoryInstaller : MonoInstaller
{
    [SerializeField] private EquipmentInventory _equipmentInventoryInstance;

    public override void InstallBindings()
    {
        Container.Bind<EquipmentInventory>().FromInstance(_equipmentInventoryInstance).AsSingle();
    }
}

