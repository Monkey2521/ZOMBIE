using UnityEngine;
using ZombieSurvival.Interfaces;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public class Expirience : IStat
    {
        [SerializeField] private ExpirienceData _data;

        private float _expirience;

        public float BaseValue => _data.BaseValue;
        public float Value => _expirience;
        public float MinValue => _data.MinValue;
        public float MaxValue => _data.MaxValue;
        public bool MaxValueIsInfinite => _data.MaxValueIsInfinite;

        public Expirience(float value)
        {
            _data = new ExpirienceData((int)value, 0, (int)value);

            Initialize();

        }

        public Expirience(ExpirienceData data)
        {
            if (data != null)
            {
                _data = data;
            }

            Initialize();
        }

        public void Initialize()
        {
            _expirience = _data.BaseValue;
        }

        public void SetValue(float value = 0)
        {
            _expirience = value;

            if (!_data.MaxValueIsInfinite && _expirience > _data.MaxValue)
                _expirience = _data.MaxValue;

            if (_expirience < _data.MinValue)
                _expirience = _data.MinValue;
        }

        public void Add(float exp)
        {
            _expirience += exp;

            if (!_data.MaxValueIsInfinite && _expirience > _data.MaxValue)
                _expirience = _data.MaxValue;

            if (_expirience < _data.MinValue)
                _expirience = _data.MinValue;
        }
    }
}