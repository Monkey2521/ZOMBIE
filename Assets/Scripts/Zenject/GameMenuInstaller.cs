using UnityEngine;

using ZombieSurvival.UI.GameMenus;

using Zenject;

public class GameMenuInstaller : MonoInstaller
{
    [SerializeField] private GameMenu _gameMenu;

    public override void InstallBindings()
    {
        Container.Bind<GameMenu>().FromInstance(_gameMenu).AsSingle();
    }
}