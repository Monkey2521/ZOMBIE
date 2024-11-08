using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Levels;
using ZombieSurvival.Rewards;
using ZombieSurvival.UI.Counters;

using Zenject;
using ZombieSurvival.Events;

namespace ZombieSurvival.UI.GameMenus
{
    public class GameMenu : MainMenu
    {
        [SerializeField] private CurrencyCounter _currencyCounter;
        [SerializeField] private SurvivalTimeCounter _survivalTimeCounter;
        [SerializeField] private MainInventory _inGameInventory;

        private List<ConcreteReward> _gameRewards;

        public SurvivalTimeCounter SurvivalTimeCounter => _survivalTimeCounter;
        public List<ConcreteReward> GameRewards => _gameRewards;

        [Inject] private LevelContext _levelContext;
        [Inject] private MainInventory _mainInventory;

        public override void Initialize()
        {
            base.Initialize();

            EventBus.Publish<IStartLevelHandler>(handler => handler.OnStartLevel());

            _gameRewards = new List<ConcreteReward>();
        }

        public override void Display(UIMenu tab)
        {
            if (tab != _defaultMenu)
            {
                StopTime();
            }
            else
            {
                RestoreTime();
            }

            foreach (UIMenu menu in _menus)
            {
                if (menu.Equals(tab))
                {

                    menu.Display(menu != _defaultMenu);
                }

                else menu.Hide();
            }
        }

        public void AddRewards(List<ConcreteReward> rewards)
        {
            foreach (ConcreteReward reward in rewards)
            {
                AddRewards(reward);
            }
        }

        public void AddRewards(ConcreteReward reward)
        {
            bool added = _inGameInventory.Add(reward);

            if (added)
            {
#if DEBUG
                if (_isDebug) Debug.Log(name + ": Add reward = " + reward);
#endif
                ConcreteReward rewardToMerge = _gameRewards.Find(item => item.AbleToMerge(reward));

                if (rewardToMerge != null)
                {
                    rewardToMerge.Merge(reward);
#if DEBUG
                    if (_isDebug) Debug.Log(name + ": Reward merged; " + "Reward amount = " + reward.Amount + "; Total = " + rewardToMerge.Amount);
#endif
                }
                else
                {
                    _gameRewards.Add(reward.Clone());
#if DEBUG
                    if (_isDebug) Debug.Log(name + ": Add new reward; Total = " + reward.Amount);
#endif
                }
            }
        }

        public void SaveLevel()
        {
            foreach(ConcreteReward reward in _gameRewards)
            {
                _mainInventory.Add(reward);
            }

            if ((int)_survivalTimeCounter.SurvivalTime > _levelContext.maxSurvivalTime)
            {
                _mainInventory.Add(new LevelSurvivalTimeReward(_levelContext, (int)_survivalTimeCounter.SurvivalTime));
            }

            _mainInventory.SaveData();
        }

        private void StopTime()
        {
            if (_isDebug) Debug.Log("Time stops now");

            Time.timeScale = 0;
        }

        private void RestoreTime()
        {
            if (_isDebug) Debug.Log("Time restored from now");

            Time.timeScale = 1;
        }
    }
}