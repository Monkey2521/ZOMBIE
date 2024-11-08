using ZombieSurvival.General;

namespace ZombieSurvival.Rewards
{
    [System.Serializable]
    public class CurrencyReward : ConcreteReward
    {
        protected Currency _currency;

        public Currency Currency => _currency;

        public CurrencyReward(CurrencyData data, int amount = 1) : base(data.Icon, data.Background, amount)
        {
            _currency = new Currency(data, amount);
        }

        public CurrencyReward(Currency currency) : this(currency.CurrencyData, currency.CurrencyValue) { }

        public override bool AbleToMerge(ConcreteReward other)
        {
            if (other is CurrencyReward compare)
            {
                return compare._currency.CurrencyData.Marker.Equals(_currency.CurrencyData.Marker);
            }

            return false;
        }

        public override ConcreteReward Clone() => new CurrencyReward(_currency.CurrencyData, _amount);
    }
}
