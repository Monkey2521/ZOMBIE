using UnityEngine;
using Zenject;
using ZombieSurvival.Characters;
using ZombieSurvival.Events;

namespace ZombieSurvival.General
{
    public class GameInitializer : MainMenuInitializer
    {
        [Inject] private Player _player;

        protected override void Start()
        {
            base.Start();

            _player.Initialize();

            EventBus.Publish<IGameStartHandler>(handler => handler.OnGameStart());
        }
    }
}