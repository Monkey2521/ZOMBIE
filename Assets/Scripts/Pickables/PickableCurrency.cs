using UnityEngine;

using ZombieSurvival.Rewards;

namespace ZombieSurvival.Objects.Pickables
{
    [RequireComponent(typeof(SphereCollider))]
    public class PickableCurrency : MonoPickableReward
    {
        [Header("Currency settings")]
        [SerializeField] private CurrencyRewardData _currency;

        protected override RewardData RewardData => _currency;
    }
}