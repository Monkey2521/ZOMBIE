using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.Equipments;

namespace ZombieSurvival.UI.InventoryMenu
{
    public class EquipmentRarityInfo : MonoBehaviour
    {
        [SerializeField] private Image _rarityCircleImage;
        [SerializeField] private Image _rarityLockedImage;
        [SerializeField] private Text _rarityDescriptionText;

        private EquipmentRarity _rarity;

        public void Initialize(EquipmentRarity rarity, Sprite rarityCircle)
        {
            _rarity = rarity;

            _rarityCircleImage.sprite = rarityCircle;
        }

        public void SetInfo(string description, EquipmentRarity rarity)
        {
            _rarityDescriptionText.text = description;

            _rarityLockedImage.enabled = rarity.RarityIndex < _rarity.RarityIndex;
        }
    }
}