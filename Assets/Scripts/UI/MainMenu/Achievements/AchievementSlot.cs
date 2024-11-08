using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.Rewards;
using ZombieSurvival.UI;

namespace ZombieSurvival.Achievements.UI
{
    public class AchievementSlot : ZSMonoBehaviour
    {
        [Header("AchievementSlot settings")]
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Text _description;

        [Space(5)]
        [SerializeField] private Button _claimButton;
        [SerializeField] private Text _claimButtonText;
        [SerializeField] private Image _rewardIconImage;

        [Space(5)]
        [SerializeField] private AchievementProgress _progressBar;

        [Space(5)]
        [SerializeField] private AchievementLevel _levelPrefab;
        [SerializeField] private Transform _levelsParent;

        [Space(5)]
        [SerializeField] private GameObject _completedPanel;
        [SerializeField] private GameObject _uncompletedPanel;

        private Achievement _achievement;
        private List<AchievementLevel> _levels;
        private MainInventory _mainInventory;
        private MainMenu _mainMenu;

        public void Initialize(Achievement achievement, MainInventory mainInventory, MainMenu mainMenu)
        {
            _achievement = achievement;
            _mainInventory = mainInventory;
            _mainMenu = mainMenu;

            _levels = new List<AchievementLevel>(_achievement.MaxLevel);

            _iconImage.sprite = _achievement.Icon;
            _backgroundImage.sprite = _achievement.Background;
            _description.text = _achievement.Name;

            for (int i = 0; i < _achievement.MaxLevel; i++)
            {
                AchievementLevel level = Instantiate(_levelPrefab, _levelsParent);

                _levels.Add(level);
            }

            UpdateSlot();
        }

        public void UpdateSlot()
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                _levels[i].Initialize(i < _achievement.Level);
            }

            AchievementData unclaimed = _achievement.UnclaimedObjective;
            AchievementData current = _achievement.CurrentObjective;

            if (unclaimed != null)
            {
                _claimButton.interactable = true;
                _claimButtonText.text = "Claim x" + unclaimed.Reward.CurrencyValue.ToString();
                _rewardIconImage.sprite = unclaimed.Reward.CurrencyData.Icon;

                AchievementData previousData = _achievement.GetPreviousData(unclaimed);
                int previousValue = previousData == null ? 0 : previousData.RequiredCount;

                _progressBar.Initialize(previousValue, unclaimed.RequiredCount);
                _progressBar.UpdateValue(_achievement.Progress);

                _uncompletedPanel.SetActive(true);
                _completedPanel.SetActive(false);
            }
            else if (current != null)
            {
                _claimButton.interactable = false;

                _claimButtonText.text = "Claim x" + current.Reward.CurrencyValue.ToString();
                _rewardIconImage.sprite = current.Reward.CurrencyData.Icon;

                AchievementData previousData = _achievement.GetPreviousData(current);
                int previousValue = previousData == null ? 0 : previousData.RequiredCount;

                _progressBar.Initialize(previousValue, current.RequiredCount);
                _progressBar.UpdateValue(_achievement.Progress);

                _uncompletedPanel.SetActive(true);
                _completedPanel.SetActive(false);
            }
            else
            {
                _uncompletedPanel.SetActive(false);
                _completedPanel.SetActive(true);

                _progressBar.Initialize(_achievement.Progress, _achievement.LastCompletedObjective.RequiredCount);
                _progressBar.UpdateValue(_achievement.Progress, isInfinite: true);
            }
        }

        public void OnClaim()
        {
            _mainMenu.ShowRewards(new CurrencyReward(_achievement.UnclaimedObjective.Reward));
            _mainInventory.Add(new AchievementObtainReward(_achievement));

            UpdateSlot();
        }
    }
}