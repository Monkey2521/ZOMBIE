using UnityEngine;

using ZombieSurvival.UI.Camp;

namespace ZombieSurvival.UI.Shop
{
    [System.Serializable]
    public class CampRoulleteChance : CirckeRouletteChance
    {
        [SerializeField] private CampBuilding _building;
        [SerializeField] private Sprite _rewardBackground;

        public CampBuilding Building => _building;
        public override Sprite RewardBackground => _rewardBackground;
    }
}