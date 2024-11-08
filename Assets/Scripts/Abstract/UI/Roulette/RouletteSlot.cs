using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.Interfaces;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.UI.Shop
{
    public abstract class RouletteSlot : ZSMonoBehaviour, IPoolable
    {
        [Header("Roulette slot settings")]
        [SerializeField] protected Image _rewardBackgroundImage;
        [SerializeField] protected Image _rewardIconImage;

        protected ConcreteReward _reward;

        public ConcreteReward Reward => _reward;

        public virtual void Initialize(ConcreteReward reward)
        {
            if (reward == null)
            {
                if (_isDebug) Debug.Log(name + ": Missing reward!");

                return;
            }

            _reward = reward;

            InitializeSlot();
        }

        public virtual void ResetObject()
        {
            _reward = null;

            _rewardBackgroundImage.sprite = null;
            _rewardIconImage.sprite = null;
        }

        protected virtual void InitializeSlot()
        {
            _rewardBackgroundImage.sprite = _reward.Background;
            _rewardIconImage.sprite = _reward.Icon;
        }
    }
}