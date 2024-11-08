using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.General.Enums;
using ZombieSurvival.General.Sounds;
using ZombieSurvival.UI;

using Zenject;
using UnityEngine.UI;

namespace ZombieSurvival.Rewards.UI
{
    public class RewardsInfo : UIMenu
    {
        [Header("RewardInfo settings")]
        [SerializeField] private SoundList _sounds;
        [SerializeField] private RewardSlot _rewardSlotPrefab;
        [SerializeField] private RectTransform _rewardSlotsParent;
        [SerializeField] private GridLayoutGroup _rewardsGrid;
        [SerializeField] private int _rewardsInRow = 5;
        [SerializeField] private bool _changeHeight;

        private List<RewardSlot> _slots = new List<RewardSlot>();

        private bool _onDisplay;
        private List<ConcreteReward> _rewardsToAdd;

        [Inject] private MainInventory _mainInventory;

        public override void Hide(bool playAnimation = false)
        {
            base.Hide(playAnimation);

            _onDisplay = false;
        }

        public void ShowReward(Reward reward, bool playSound = true, bool addToInventory = true)
        {
            ShowReward(reward.Rewards, playSound, addToInventory);
        }

        public void ShowReward(List<ConcreteReward> rewards, bool playSound = true, bool addToInventory = true)
        {
            if (!_onDisplay)
            {
                _rewardsToAdd = new List<ConcreteReward>();
            }

            _onDisplay = true;

            if (_slots.Count > 0)
            {
                foreach (var slot in _slots)
                {
                    Destroy(slot.gameObject);
                }
                _slots.Clear();
            }

            foreach (ConcreteReward reward in rewards)
            {
                if (reward == null || reward.Amount < 1)
                {
                    if (_isDebug) Debug.Log("Missing reward!");

                    continue;
                }

                ConcreteReward rewardToMerge = _rewardsToAdd.Find(item => item.AbleToMerge(reward));

                if (rewardToMerge != null)
                {
                    rewardToMerge.Merge(reward);
                }
                else
                {
                    _rewardsToAdd.Add(reward.Clone());
                }

                if (addToInventory)
                {
                    _mainInventory.Add(reward);
                }
            }

            foreach (ConcreteReward reward in _rewardsToAdd)
            {
                if (reward.Icon != null)
                {
                    RewardSlot slot = Instantiate(_rewardSlotPrefab, _rewardSlotsParent);

                    slot.Initialize(reward);

                    _slots.Add(slot);
                }
            }

            if (_changeHeight)
            {
                _rewardSlotsParent.sizeDelta = new Vector2(0, GetInventoryHeight(_rewardsToAdd.Count));
            }


            if (playSound)
            {
                _sounds.PlaySound(SoundTypes.GetReward);
            }
        }


        private int GetInventoryHeight(int rewardsAmount)
        {
            int rows = rewardsAmount / _rewardsInRow + (rewardsAmount % _rewardsInRow > 0 ? 1 : 0);

            return _rewardsGrid.padding.top +
                   _rewardsGrid.padding.bottom +
                   (int)_rewardsGrid.cellSize.y * rows +
                   (int)_rewardsGrid.spacing.y * rows;
        }
    }
}