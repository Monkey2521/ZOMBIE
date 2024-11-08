using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.General.Enums;
using ZombieSurvival.General.Sounds;
using ZombieSurvival.Levels;
using ZombieSurvival.Rewards.UI;
using ZombieSurvival.UI.Counters;

namespace ZombieSurvival.UI.GameMenus
{
    public sealed class LevelPassedMenu : UIMenu, ILevelPassedHandler
    {
        [Header("Level passed menu settings")]
        [SerializeField] private SoundList _sounds;

        [Space(5)]
        [SerializeField] private Text _totalKilledText;

        [Space(5)]
        [SerializeField] private CurrencyCounter _currencyCounter;
        [SerializeField] private ZombieCounter _enemyCounter;
        [SerializeField] private RewardsInfo _rewardsInfo;

        private bool _onLoad;

        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public override void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            base.Initialize(mainMenu, parentMenu);

            _totalKilledText.text = "0";

            _onLoad = false;

            _rewardsInfo.Initialize(mainMenu, this);
        }

        public void OnLevelPassed(LevelContext level)
        {
            _mainMenu.Display(this);

            Display(true);

            if (_mainMenu is GameMenu gameMenu)
            {
                _rewardsInfo.Display();
                _rewardsInfo.ShowReward(gameMenu.GameRewards, playSound: false, addToInventory: false);
            }
            else
            {
                _rewardsInfo.Hide();
            }

            _totalKilledText.text = _enemyCounter.TotalKilled.ToString();

            EventBus.Publish<ISurvivalTimeHandler>(handler => handler.OnSurvivalTime
                                    ((int)(_mainMenu as GameMenu).SurvivalTimeCounter.SurvivalTime));

            (_mainMenu as GameMenu).SaveLevel();

            _sounds.PlaySound(SoundTypes.LevelPassed);
        }

        public void OnContinue()
        {
            if (_onLoad) return;

            _onLoad = true;

            var operation = SceneManager.LoadSceneAsync(GameData.MainMenuScene);

            operation.completed += new System.Action<AsyncOperation>((operation) => Time.timeScale = 1);
        }
    }
}