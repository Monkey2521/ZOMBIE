using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Rewards;
using ZombieSurvival.UI.GameMenus;

namespace ZombieSurvival.Objects.Pickables
{
    public abstract class MonoPickableReward : PickableObject 
    {
        [Header("Pickable reward settings")]
        [Tooltip("Background will not change on initialization if true")]
        [SerializeField] protected bool _haveCustomBackground;
        [Tooltip("Field can be null HaveCustomBackground is true")]
        [SerializeField] protected SpriteRenderer _rewardBackgroundSprite;
        
        [Space(5)]
        [Tooltip("Icon will not change on initialization if true")]
        [SerializeField] protected bool _haveCustomIcon;
        [Tooltip("Field can be null HaveCustomIcon is true")]
        [SerializeField] protected SpriteRenderer _rewardIconSprite;

        protected GameMenu _gameMenu;

        protected ConcreteReward _reward;

        protected abstract RewardData RewardData { get; }

        public virtual void Initialize(MonoPool<PickableObject> pool, GameMenu gameMenu)
        {
            base.Initialize(pool);

            _gameMenu = gameMenu;

            List<ConcreteReward> rewards = RewardData.GetConcreteRewards();

            if (rewards.Count == 1)
            {
                _reward = rewards[0];

                if (!_haveCustomBackground && _rewardBackgroundSprite != null)
                {
                    _rewardBackgroundSprite.sprite = _reward.Background;
                }
                
                if (!_haveCustomIcon && _rewardIconSprite != null)
                {
                    _rewardIconSprite.sprite = _reward.Icon;
                }
            }
            else
            {
                if (_isDebug) Debug.Log(name + ": Reward amount error!");

                if (_pool != null)
                {
                    _pool.Release(this);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        protected override void OnPickUp(float releaseDelay = 0)
        {
            _gameMenu.AddRewards(_reward);

            base.OnPickUp(releaseDelay);
        }
    }
}