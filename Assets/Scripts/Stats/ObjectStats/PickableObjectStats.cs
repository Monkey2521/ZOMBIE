using UnityEngine;
using ZombieSurvival.Interfaces;
using ZombieSurvival.Stats;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Objects
{
    [System.Serializable]
    public class PickableObjectStats : IObjectStats
    {
        [SerializeField] protected PickUpRadius _pickUpRadius;
        [SerializeField] protected PickUpSpeed _pickUpSpeed;
        [SerializeField] protected Duration _knockbackDuration;
        [SerializeField] protected PickUpSpeed _knockbackSpeed;

        public PickUpRadius PickUpRadius => _pickUpRadius;
        public PickUpSpeed PickUpSpeed => _pickUpSpeed;
        public Duration KnockbackDuration => _knockbackDuration;
        public PickUpSpeed KnockbackSpeed => _knockbackSpeed;

        public void Initialize()
        {
            _pickUpRadius.Initialize();
            _pickUpSpeed.Initialize();
            _knockbackDuration.Initialize();
            _knockbackSpeed.Initialize();
        }

        public void GetUpgrade(Upgrade upgrade)
        {
            _pickUpRadius.Upgrade(upgrade);
            _pickUpSpeed.Upgrade(upgrade);
            _knockbackDuration.Upgrade(upgrade);
            _knockbackSpeed.Upgrade(upgrade);
        }

        public void DispelUpgrade(Upgrade upgrade)
        {
            _pickUpRadius.DispelUpgrade(upgrade);
            _pickUpSpeed.DispelUpgrade(upgrade);
            _knockbackDuration.DispelUpgrade(upgrade);
            _knockbackSpeed.DispelUpgrade(upgrade);
        }
    }
}