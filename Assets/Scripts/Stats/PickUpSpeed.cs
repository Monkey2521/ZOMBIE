using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public class PickUpSpeed : Speed
    {
        public PickUpSpeed(StatData statData, UpgradeList upgradeList = null, bool isDebug = false) :
            base(statData, upgradeList, isDebug)
        {
            _value = (_statData.BaseValue + _upgrades.UpgradesValue) * _upgrades.UpgradesMultiplier;

            if (_value < _minValue) _value = _minValue;
            if (!_statData.MaxValueIsInfinite && _value > _maxValue) _value = _maxValue;
        }
    }
}