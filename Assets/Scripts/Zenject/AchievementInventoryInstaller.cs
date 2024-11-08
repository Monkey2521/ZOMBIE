using UnityEngine;

using ZombieSurvival.General.Inventories;

using Zenject;

public class AchievementInventoryInstaller : MonoInstaller
{
    [SerializeField] private AchievementInventory _achievementInventoryInstance;

    public override void InstallBindings()
    {
        Container.Bind<AchievementInventory>().FromInstance(_achievementInventoryInstance).AsSingle();
    }
}

