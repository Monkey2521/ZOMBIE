using System;
using System.Collections.Generic;

using UnityEngine;
using ZombieSurvival.Events;
using ZombieSurvival.General;

namespace ZombieSurvival.UI.Shop
{
    public sealed class ShopMenu : UIMenu
    {
        [SerializeField] private ShopGrid _shopGrid;

        public override void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            base.Initialize(mainMenu, parentMenu);

            _shopGrid.Initialize();
        }

        public void DisplayTab(ShopTab tab, bool displayByMainMenu = true)
        {
            EventBus.Publish<IOpenShopHandler>(handler => handler.OnOpenShop());

            if (displayByMainMenu)
            {
                MainMenuDisplay();
            }
            else
            {
                Display();
            }

            _shopGrid.TabsParent.localPosition = new Vector3(0, -_shopGrid[tab] - tab.Height * 0.5f);
        }
        
        public void DisplayTab(ShopTab tab)
        {
            DisplayTab(tab, displayByMainMenu: true);
        }

        [Serializable]
        private class ShopGrid
        {
            [SerializeField] private List<ShopTab> _tabs;
            [SerializeField] private RectTransform _tabsParent;

            [SerializeField] private int _topPadding;
            [SerializeField] private int _bottomPadding;
            [SerializeField] private int _tabsSpacingY;

            private Dictionary<ShopTab, int> _tabsDeltaPos;

            public RectTransform TabsParent => _tabsParent;
            
            public int this[ShopTab tab]
            {
                get
                {
                    if (_tabsDeltaPos.ContainsKey(tab))
                    {
                        return _tabsDeltaPos[tab];
                    }

                    return 0;
                }
            }

            public void Initialize()
            {
                foreach (ShopTab tab in _tabs)
                {
                    tab.Initialize();
                }
                
                _tabsDeltaPos = new Dictionary<ShopTab, int>();
                _tabsParent.sizeDelta = new Vector2(0f, GetViewPortHeight());

                _topPadding = (int)(_topPadding * ScreenScaler.MinDelta);
                _bottomPadding = (int)(_bottomPadding * ScreenScaler.MinDelta);
                _tabsSpacingY = (int)(_tabsSpacingY * ScreenScaler.MinDelta);

                foreach (ShopTab tab in _tabs)
                {
                    tab.transform.localPosition = new Vector3(Screen.width * 0.5f, _tabsDeltaPos[tab]);
                }
            }

            private float GetViewPortHeight()
            {
                int height = _topPadding;

                for(int i = 0; i < _tabs.Count; i++)
                {
                    if (_tabsDeltaPos.ContainsKey(_tabs[i]))
                    {
                        _tabsDeltaPos[_tabs[i]] = (int)(-height - _tabs[i].Height * 0.5f);
                    }
                    else
                    {
                        _tabsDeltaPos.Add(_tabs[i], (int)(-height - _tabs[i].Height * 0.5f));
                    }

                    height += _tabs[i].Height;

                    if (i < _tabs.Count - 1)
                    {
                        height += _tabsSpacingY;
                    }
                    else
                    {
                        height += _bottomPadding;
                    }
                }

                return height;
            }
        }
    }
}