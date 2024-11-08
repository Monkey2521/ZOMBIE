using ZombieSurvival.General;
using ZombieSurvival.UI.Shop;

namespace ZombieSurvival.Rewards
{
    public class CircleRouletteCurrencyReward : CircleRouletteReward
    {
        private CurrencyRouletteChance _currencyChance;

        public Currency Currency => _currencyChance.Currency;

        public CircleRouletteCurrencyReward(CurrencyRouletteChance currencyChance, CircleRouletteSlotData slotData) :
            base(currencyChance.Currency.CurrencyData.Icon, currencyChance.Currency.CurrencyData.Background, slotData, currencyChance.Currency.CurrencyValue)
        {
            _currencyChance = currencyChance;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return false;
        }

        public override ConcreteReward Clone() => new CircleRouletteCurrencyReward(_currencyChance, _slotData);
    }
}
