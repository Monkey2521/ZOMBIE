using UnityEngine;

using Zenject;

using ZombieSurvival.Rewards;
using ZombieSurvival.UI.GameMenus;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.General.Inventories
{
    public class UpgradeableCurrencyInventory : UpgradeableInventory
    {
        [Header("CurrencyInventory settings")]
        [SerializeField] protected CurrencyData _currencyData;
        
        [Inject] protected GameMenu _gameMenu;

        protected override void OnGetUpgrade(UpgradeData data)
        {
            _gameMenu.AddRewards(new CurrencyReward(_currencyData, (int)(data.UpgradeValue * data.UpgradeMultiplier)));
        }
    }
}