using UnityEngine;
using ZombieSurvival.Characters;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Objects.Pickables
{
    [RequireComponent(typeof(SphereCollider))]
    public class PickableHealth : PickableObject
    {
        [Header("AidBag settings")]
        [SerializeField] private Upgrade _healUpgrade;

        protected override void OnPickUp(float releaseDelay = 0)
        {
            if (_target.TryGetComponent(out Player player))
            {
                player.GetUpgrade(_healUpgrade);
            }
            else if (_isDebug) Debug.Log("Missing player!");

            base.OnPickUp(releaseDelay);
        }
    }
}