using UnityEngine;
using ZombieSurvival.General;

namespace ZombieSurvival.Equipments
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Equipment/Equipment rarity", fileName = "New equipment rarity")]
    public class EquipmentRarity : Marker
    {
        [SerializeField][Range(0, 5)] private int _rarityIndex;

        public int RarityIndex => _rarityIndex;

        public override string ToString()
        {
            return name;
        }
    }
}