using UnityEngine;

using ZombieSurvival.Characters;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.Objects.Pickables
{
    [RequireComponent(typeof(SphereCollider))]
    public sealed class PickableCoin : PickableCurrency
    {
        protected override void OnPickUp(float releaseDelay = 0)
        {
            if (_target != null)
            {
                if (_target.TryGetComponent(out Player player))
                {
                    //player.CoinInventory.Add((_reward as CurrencyReward).Currency);
                }
            }

            base.OnPickUp(releaseDelay);
        }
    }
}