using UnityEngine;

namespace ZombieSurvival.General
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Currency/Currency data", fileName = "New currency data")]
    public class CurrencyData : ScriptableObject
    {
        [SerializeField] protected Sprite _icon;
        [SerializeField] protected Sprite _background;
        [SerializeField] protected CurrencyMarker _marker;

        public Sprite Icon => _icon;
        public Sprite Background => _background;
        public CurrencyMarker Marker => _marker;
    }
}