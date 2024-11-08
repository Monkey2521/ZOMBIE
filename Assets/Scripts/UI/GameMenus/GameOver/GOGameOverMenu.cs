using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.General.Enums;
using ZombieSurvival.General.Sounds;
using ZombieSurvival.Levels;
using ZombieSurvival.Rewards;
using ZombieSurvival.Rewards.UI;
using ZombieSurvival.UI.Counters;

namespace ZombieSurvival.UI.GameMenus.GameOver
{
    public class GOGameOverMenu : UIMenu
    {
        [Header("Game over menu settings")]
        [SerializeField] private SoundList _sounds;

        [Space(5)]
        [SerializeField] private Text _levelNameText;
        [SerializeField] private RewardsInfo _rewardsInfo;

        [Space(5)]
        [SerializeField] private ZombieCounter _enemyCounter;
        [SerializeField] private SurvivalTimeCounter _survivalTimeCounter;
        [SerializeField] private CurrencyCounter _currencyCounter;

        [SerializeField] private Text _totalKilledText;
        [SerializeField] private Text _survivalTimeText;

        [Inject] private LevelContext _levelContext;

        public override void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            base.Initialize(mainMenu, parentMenu);

            _rewardsInfo.Initialize(mainMenu, this);
        }

        public override void Display(bool playAnimation = false)
        {
            base.Display(playAnimation);

            _levelNameText.text = _levelContext.LevelNumber.ToString() + ". " + _levelContext.LevelName;

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
            _survivalTimeText.text = IntegerFormatter.GetTime((int)_survivalTimeCounter.SurvivalTime);

            EventBus.Publish<ISurvivalTimeHandler>(handler => handler.OnSurvivalTime((int)_survivalTimeCounter.SurvivalTime));

            (_mainMenu as GameMenu).SaveLevel();

            _sounds.PlaySound(SoundTypes.GameOver);
        }
    }
}