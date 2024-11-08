using ZombieSurvival.Objects.Indication;

namespace ZombieSurvival.Objects
{
    public struct ReceivedDamage 
    {
        private int _receivedDamage;
        private ObjectDamagedIndicator _damageIndicator;

        public int Value => _receivedDamage;
        public ObjectDamagedIndicator Indicator => _damageIndicator;

        public ReceivedDamage(int receivedDamage, ObjectDamagedIndicator damageIndicator)
        {
            _receivedDamage = receivedDamage;
            _damageIndicator = damageIndicator;
        }
    }
}