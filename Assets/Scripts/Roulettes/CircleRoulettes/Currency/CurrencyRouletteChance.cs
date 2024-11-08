using UnityEngine;
using ZombieSurvival.General;

namespace ZombieSurvival.UI.Shop
{
    [System.Serializable]
    public class CurrencyRouletteChance : CirckeRouletteChance
    {
        [SerializeField] private Currency _currency;

        public override Sprite RewardBackground => _currency.CurrencyData.Background;

        public Currency Currency => _currency;

    }
}