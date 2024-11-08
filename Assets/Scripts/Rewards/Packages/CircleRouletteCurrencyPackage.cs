using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.UI.Shop
{
    public class CircleRouletteCurrencyPackage : ShopPackage
    {
        [Header("Roulette settings")]
        [SerializeField] private RouletteMarker _rouletteMarker;
        [SerializeField] private ChanceCombiner<CurrencyRouletteChance> _chances;

        [Space(5)]
        [SerializeField] private bool _enableAdReward;
        [SerializeField] private GameObject _adButton;

        public override void Initialize()
        {
            base.Initialize();

            _chances.Initialize();

            if (!_enableAdReward)
            {
                _adButton?.SetActive(false);
            }
            else
            {
                _adButton.SetActive(true);
            }
        }

        public override void OnBuyClick()
        {
            if (_mainInventory.Spend(new CurrencyReward(_cost)))
            {
                if (!_mainMenu.DisplayRoulette(_chances, _rouletteMarker))
                {
                    _mainMenu.ShowRewards(new CurrencyReward(_chances.GetStrikedObject().Currency));
                }
            }
            else
            {
                _mainMenu.ShowPopupMessage("Not enough resources!");
            }
        }

        public void OnAdClick()
        {
            if (!_mainMenu.DisplayRoulette(_chances, _rouletteMarker))
            {
                _mainMenu.ShowRewards(new CurrencyReward(_chances.GetStrikedObject().Currency));
            }
        }
    }
}