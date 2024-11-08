using UnityEngine;
using UnityEngine.UI;

using Zenject;

using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.Levels;
using ZombieSurvival.UI.GameMenus;
using ZombieSurvival.UI.General;

namespace ZombieSurvival.UI.Counters
{
    public class SurvivalTimeCounter : ZSMonoBehaviour, IGameStartHandler, IBossEventHandler, IBossEventEndedHandler
    {
        [Header("SurvivalTimeCounter settings")]
        [SerializeField] private LevelProgress _levelProgress;
        [SerializeField] private Text _survivalTimeText;

        private float _survivalTime;
        private float _expirienceTimer;
        private bool _onBossEvent;

        public float SurvivalTime => _survivalTime;

        [Inject] private GameMenu _gameMenu;
        [Inject] private LevelContext _levelContext;

        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        private void FixedUpdate()
        {
            if (_onBossEvent) return;

            _survivalTime += Time.fixedDeltaTime;
            _expirienceTimer += Time.fixedDeltaTime;

            if (_expirienceTimer >= _levelContext.TickTime)
            {
                _gameMenu.AddRewards(_levelContext.ExpirienceRewardPerTick.GetConcreteRewards());

                _expirienceTimer -= _levelContext.TickTime;
            }

            _levelProgress.OnTimerUpdate(_survivalTime);

            UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            _survivalTimeText.text = IntegerFormatter.GetTime((int)_survivalTime);
        }

        public void OnGameStart()
        {
            _survivalTime = 0f;
            _expirienceTimer = 0f;
            UpdateTimerText();
        }

        public void OnBossEvent()
        {
            _onBossEvent = true;
        }

        public void OnBossEventEnd()
        {
            _onBossEvent = false;
        }
    }
}