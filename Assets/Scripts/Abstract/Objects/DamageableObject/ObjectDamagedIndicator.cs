using UnityEngine;

namespace ZombieSurvival.Objects.Indication
{
    public abstract class ObjectDamagedIndicator : ObjectIndicator
    {
        [Header("ObjectDamaged indicator settings")]
        [SerializeField] protected DamageableObject _indicationObject;

        protected override float GetIndicationIntensivity()
        {
            float intensivity = 1 - _indicationObject.Stats.Health.Value / _indicationObject.Stats.Health.MaxValue;

            if (intensivity < _minIndicationIntensivity)
            {
                intensivity = _minIndicationIntensivity;
            }

            if (intensivity > MAX_INDICATION_INTENSIVITY)
            {
                intensivity = MAX_INDICATION_INTENSIVITY;
            }

            return intensivity;
        }
    }
}