using ZombieSurvival.Stats;

namespace ZombieSurvival.UI
{
    public class HPBar : FillBar
    {
        private Health _health;

        public void Initialize(Health health)
        {
            _health = health;

            _maxFillValue = (int)_health.MaxValue;
            _minFillValue = (int)_health.MinValue;
            _value = _maxFillValue;

            base.Initialize();
        }

        public void UpdateHealth()
        {
            _value = (int)_health.Value;
            _maxFillValue = (int)_health.MaxValue;

            base.Initialize();
        }
    }
}