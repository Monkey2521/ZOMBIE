using UnityEngine;

using ZombieSurvival.General;

using Zenject;

public class PlayerLevelInventoryInstaller : MonoInstaller
{
    [SerializeField] private PlayerLevelInventory _playerLevelInventoryInstance;

    public override void InstallBindings()
    {
        Container.Bind<PlayerLevelInventory>().FromInstance(_playerLevelInventoryInstance).AsSingle();
    }
}
