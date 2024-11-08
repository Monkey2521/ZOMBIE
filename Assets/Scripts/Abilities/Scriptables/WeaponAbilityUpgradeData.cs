using UnityEngine;

namespace ZombieSurvival.Abilities
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Abilities/Weapon ability upgrade data", fileName = "New weapon ability upgrade data")]
    public class WeaponAbilityUpgradeData : AbilityUpgradeData
    {
        [SerializeField] protected Weapon _weapon;

        public Weapon Weapon => _weapon;
    }
}