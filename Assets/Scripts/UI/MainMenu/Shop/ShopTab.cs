using System.Collections.Generic;
using UnityEngine;
using ZombieSurvival.General;

namespace ZombieSurvival.UI.Shop
{
    public class ShopTab : MonoBehaviour
    {
        [SerializeField] private RectTransform _selfTransform;
        [SerializeField] private int _tabHeight;
        [SerializeField] private List<ShopPackage> _packages;

        public int Height => _tabHeight;

        public void Initialize()
        {
            _selfTransform.sizeDelta = new Vector2
                    (
                        0,
                        _tabHeight * ScreenScaler.MinDelta
                    );

            _tabHeight = (int)(_tabHeight * ScreenScaler.MinDelta);

            foreach (ShopPackage package in _packages)
            {
                package.Initialize();
            }
        }
    }
}