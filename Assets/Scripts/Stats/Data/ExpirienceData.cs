using UnityEngine;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public class ExpirienceData
    {
        [SerializeField] private int _baseValue;
        [SerializeField] private float _minValue;
        [SerializeField] private int _maxValue;
        [SerializeField] private bool _maxValueIsInfinite;

        public int BaseValue => _baseValue;
        public float MinValue => _minValue;
        public int MaxValue => _maxValue;
        public bool MaxValueIsInfinite => _maxValueIsInfinite;

        public ExpirienceData(int baseValue, int minValue, int maxValue, bool maxValueIsInfinite = false)
        {
            _baseValue = baseValue;
            _minValue = minValue;
            _maxValue = maxValue;
            _maxValueIsInfinite = maxValueIsInfinite;
        }
    }
}