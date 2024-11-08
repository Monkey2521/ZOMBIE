using UnityEngine;
using ZombieSurvival.UI.Shop;

namespace ZombieSurvival.UI
{
    public sealed class ShopBuyTutorialButton : TutorialButton
    {
        [SerializeField] private ShopMenu _shopMenu;
        [SerializeField] private ShopTab _tabToDisplay;

        public override void OnTutorialEnd()
        {
            base.OnTutorialEnd();

            _shopMenu.DisplayTab(_tabToDisplay);
        }
    }
}