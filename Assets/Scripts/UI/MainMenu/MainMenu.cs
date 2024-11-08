using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Rewards;
using ZombieSurvival.Rewards.UI;
using ZombieSurvival.UI.General;
using ZombieSurvival.UI.Shop;

namespace ZombieSurvival.UI
{
    public class MainMenu : ZSMonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] protected UIMenu _defaultMenu;
        [SerializeField] protected List<UIMenu> _menus;

        [SerializeField] private SettingsMenu _settings;

        [Space(5)]
        [SerializeField] private PopupMessage _message;
        [SerializeField] private List<Roulette> _roulettes;
        [SerializeField] private RewardsInfo _rewardsInfo;

        public virtual void Initialize()
        {
            foreach (UIMenu menu in _menus)
            {
                menu.Initialize(this);

                if (menu.Equals(_defaultMenu))
                {
                    menu.Display();
                }

                else menu.Hide();
            }

            _settings?.Initialize();
            _settings?.Hide();

            _message?.Initialize(this);
            _message?.Hide();

            if (_roulettes != null)
            {
                foreach (Roulette roulette in _roulettes)
                {
                    if (roulette != null)
                    {
                        roulette.Initialize(this);
                        roulette.Hide();
                    }
                }
            }

            _rewardsInfo?.Initialize(this);
            _rewardsInfo?.Hide();
        }

        public virtual void Display(UIMenu tab)
        {
            foreach (UIMenu menu in _menus)
            {
                if (menu.Equals(tab) && !menu.Enabled)
                {
                    menu.Display();
                }
                else if (menu.Enabled && !menu.Equals(tab))
                {
                    menu.Hide();
                }
            }
        }

        private void FixedUpdate()
        {
            if (_roulettes == null) return;

            foreach (Roulette roulette in _roulettes)
            {
                if (roulette != null)
                {
                    roulette.OnFixedUpdate();
                }
            }
        }

        public void DisplayDefault()
        {
            Display(_defaultMenu);
        }

        public void OnSettingsClick()
        {
            _settings?.Display(true);
        }

        public void OnSettingsClose()
        {
            _settings?.Hide(true);
        }

        public bool ShowRewards(Reward reward)
        {
            return ShowRewards(reward.Rewards);
        }

        public bool ShowRewards(List<ConcreteReward> rewards)
        {
            if (_rewardsInfo == null) return false;

            _rewardsInfo?.ShowReward(rewards);
            _rewardsInfo?.Display(true);

            return true;
        }

        public bool ShowRewards(ConcreteReward reward)
        {
            if (reward == null || reward.Amount == 0) return false;

            List<ConcreteReward> rewards = new List<ConcreteReward>();

            rewards.Add(reward);

            return ShowRewards(rewards);
        }

        public void ShowPopupMessage(string message)
        {
            _message.ShowMessage(message);
        }

        public bool DisplayRoulette<T>(ChanceCombiner<T> chances, RouletteMarker rouletteMarker,
                                       int roulettesCount = 1, bool autoStart = true)
            where T : class
        {
            if (_roulettes == null) return false;

            Roulette roulette = _roulettes.Find(item => item.Marker.Equals(rouletteMarker));

            if (roulette == null) return false;

            roulette.Display(playAnimation: true);
            roulette.DisplayRoulette(chances, roulettesCount, autoStart);

            return true;
        }
    }
}