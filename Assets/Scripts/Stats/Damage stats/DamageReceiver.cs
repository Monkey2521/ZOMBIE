using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Objects;
using ZombieSurvival.Objects.Indication;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public sealed class DamageReceiver : Stat
    {
        [Header("ReceiverCombiner settings")]
        [Tooltip("Indication displaing if receiver's indication is NULL")]
        [SerializeField] private ObjectDieIndicator _defaultDieIndicator;
        [SerializeField] private List<DieIndicatorCombiner> _dieIndicators;

        [Tooltip("Indication displaing if receiver's indication is NULL")]
        [SerializeField] private ObjectDamagedIndicator _defaultDamageIndicator;
        [SerializeField] private List<ReceiverCombiner> _damageReceivers;

        public DamageReceiver(StatData statData, List<ReceiverCombiner> damageReceivers,
                                    UpgradeList upgradeList = null, bool isDebug = false) : 
            base(statData, upgradeList, isDebug)
        {
            _value = (_statData.BaseValue + _upgrades.UpgradesValue) * _upgrades.UpgradesMultiplier;

            if (_value < _minValue) _value = _minValue;
            if (!_statData.MaxValueIsInfinite && _value > _maxValue) _value = _maxValue;

            _damageReceivers = damageReceivers;

            foreach (ReceiverCombiner receiver in _damageReceivers)
            {
                receiver.Initilaize();
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach(DieIndicatorCombiner dieCombiner in _dieIndicators)
            {
                dieCombiner.Initilaize();
            }
            
            foreach(ReceiverCombiner receiver in _damageReceivers)
            {
                receiver.Initilaize();
            }
        }

        public override bool Upgrade(Upgrade upgrade)
        {
            bool receiverUpgrade = false;

            foreach(ReceiverCombiner receiver in _damageReceivers)
            {
                if (!receiverUpgrade)
                {
                    receiverUpgrade = receiver.DamageResistance.Upgrade(upgrade);
                }
                else receiver.DamageResistance.Upgrade(upgrade);
            }

            if (base.Upgrade(upgrade))
            {
                _value = (_statData.BaseValue + _upgrades.UpgradesValue) * _upgrades.UpgradesMultiplier;

                if (_value < _minValue) _value = _minValue;
                if (!_statData.MaxValueIsInfinite && _value > _maxValue) _value = _maxValue;

                return true;
            }
            else return receiverUpgrade;
        }

        public override bool DispelUpgrade(Upgrade upgrade)
        {
            bool receiverUpgrade = false;

            foreach (ReceiverCombiner receiver in _damageReceivers)
            {
                if (!receiverUpgrade)
                {
                    receiverUpgrade = receiver.DamageResistance.DispelUpgrade(upgrade);
                }
                else receiver.DamageResistance.DispelUpgrade(upgrade);
            }

            if (base.DispelUpgrade(upgrade))
            {
                _value = (_statData.BaseValue + _upgrades.UpgradesValue) * _upgrades.UpgradesMultiplier;

                if (_value < _minValue) _value = _minValue;
                if (!_statData.MaxValueIsInfinite && _value > _maxValue) _value = _maxValue;

                return true;
            }
            else return receiverUpgrade;
        }

        public ObjectDieIndicator GetDieIndicator(ConcreteDamage damage)
        {
            DieIndicatorCombiner combiner = _dieIndicators.Find(item => item.DamageMarker.Equals(damage.DamageMarker));

            if (combiner == null)
            {
                return _defaultDieIndicator;
            }

            ObjectDieIndicator indicator = combiner.RandomIndicator;

            if (indicator == null)
            {
                indicator = _defaultDieIndicator;
            }

            return indicator;
        }

        public ReceivedDamage GetDamage(ConcreteDamage damage)
        {
            ReceiverCombiner receiver = _damageReceivers.Find(item => 
                item.DamageResistance.ResistanceMarker.Equals(damage.DamageMarker));

            if (receiver == null)
            {
                return new ReceivedDamage(damage.Value, _defaultDamageIndicator);
            }

            ObjectDamagedIndicator indicator = receiver.RandomIndicator;

            return new ReceivedDamage
                (
                    (int)((damage.Value - _value) * (1 - receiver.DamageResistance.Value)),
                    indicator == null ? _defaultDamageIndicator : indicator
                );
        }

        [System.Serializable]
        public sealed class ReceiverCombiner
        {
            [SerializeField] private DamageResistance _damageResistance;
            [SerializeField] private ChanceCombiner<ObjectDamagedIndicator> _damageIndicatorChances;
            
            public DamageResistance DamageResistance => _damageResistance;
            public ObjectDamagedIndicator RandomIndicator => _damageIndicatorChances.GetStrikedObject(); 

            public void Initilaize()
            {
                _damageResistance.Initialize();

                _damageIndicatorChances.Initialize();
            } 
        }
        
        [System.Serializable]
        public sealed class DieIndicatorCombiner
        {
            [SerializeField] private DamageMarker _damageMarker;
            [SerializeField] private ChanceCombiner<ObjectDieIndicator> _dieIndicatorChances;

            public DamageMarker DamageMarker => _damageMarker;
            public ObjectDieIndicator RandomIndicator => _dieIndicatorChances.GetStrikedObject(); 

            public void Initilaize()
            {
                _dieIndicatorChances.Initialize();
            } 
        }
    }
}