using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Stats
{
    public sealed class ConcreteDamage
    {
        private int _damageValue;
        private bool _isCritical;
        private DamageMarker _damageMarker;

        public int Value => _damageValue;
        public bool IsCritical => _isCritical;
        public DamageMarker DamageMarker => _damageMarker;

        public ConcreteDamage(int damageValue, bool isCritical, DamageMarker damageMarker)
        {
            _damageValue = damageValue;
            _isCritical = isCritical;
            _damageMarker = damageMarker;
        }
    }
}