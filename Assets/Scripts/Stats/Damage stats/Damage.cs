using UnityEngine;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public class Damage : Stat
    {
        [Header("Damage settings")]
        [SerializeField] protected DamageMarker _dealingDamageMarker;
        [SerializeField] protected CriticalDamage _criticalDamage;

        /// <summary>
        /// Damage value with calculating chance to crit
        /// </summary>
        public override float Value
        {
            get
            {
                if (_criticalDamage.CritRate.IsStrike)
                {
                    return _value * _criticalDamage.Value;
                }
                else return _value;
            }
        }

        /// <summary>
        /// Damage value without Crit
        /// </summary>
        public int DamageValue => (int)_value;
        /// <summary>
        /// Damage value with Crit
        /// </summary>
        public int CriticalValue => (int)(_value * _criticalDamage.Value);

        /// <summary>
        /// Chance to deal critical damage
        /// </summary>
        public Chance CritRate => _criticalDamage.CritRate;
        /// <summary>
        /// Critical damage multiplier
        /// </summary>
        public CriticalDamage Crit => _criticalDamage;

        public override void Initialize()
        {
            base.Initialize();

            _criticalDamage.Initialize();
        }

        public Damage(StatData damageStatData, StatData critStatData, StatData chanceStatData,
                      UpgradeList damageUpgradeList = null, UpgradeList critUpgradeList = null, UpgradeList chanceUpgradeList = null,
                      bool isDebug = false) : 
            base(damageStatData, damageUpgradeList, isDebug)
        {
            _criticalDamage = new CriticalDamage(critStatData, chanceStatData, critUpgradeList, chanceUpgradeList, isDebug);

            _value = (_statData.BaseValue + _upgrades.UpgradesValue) * _upgrades.UpgradesMultiplier;

            if (_value < _minValue) _value = _minValue;
            if (!_statData.MaxValueIsInfinite && _value > _maxValue) _value = _maxValue;
        }

        public override bool Upgrade(Upgrade upgrade)
        {
            bool critUpgrade = _criticalDamage.Upgrade(upgrade);

            if (base.Upgrade(upgrade))
            {
                _value = (_statData.BaseValue + _upgrades.UpgradesValue) * _upgrades.UpgradesMultiplier;

                if (_value < _minValue) _value = _minValue;
                if (!_statData.MaxValueIsInfinite && _value > _maxValue) _value = _maxValue;

                return true;
            }
            else return critUpgrade;
        }

        public override bool DispelUpgrade(Upgrade upgrade)
        {
            bool critDispel = _criticalDamage.DispelUpgrade(upgrade);

            if (base.DispelUpgrade(upgrade))
            {
                _value = (_statData.BaseValue + _upgrades.UpgradesValue) * _upgrades.UpgradesMultiplier;

                if (_value < _minValue) _value = _minValue;
                if (!_statData.MaxValueIsInfinite && _value > _maxValue) _value = _maxValue;

                return true;
            }
            else return critDispel;
        }

        public ConcreteDamage GetValue()
        {
            int value = (int)Value;

            return new ConcreteDamage(value, value > _value, _dealingDamageMarker);
        }
    }
}