using ZombieSurvival.UI.Shop;

namespace ZombieSurvival.Rewards
{
    public class CampUpgradeReward : CircleRouletteReward
    {
        private CampRoulleteChance _chanceData;

        public CampRoulleteChance ChanceData => _chanceData;

        public CampUpgradeReward(CampRoulleteChance chanceData, CircleRouletteSlotData slotData, int amount = 1) :
            base(chanceData.Building.CampUpgrade.UnlockedIcon, chanceData.RewardBackground, slotData, amount)
        {
            _chanceData = chanceData;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return false;
        }

        public override ConcreteReward Clone() => new CampUpgradeReward(_chanceData, _slotData, _amount);
    }
}
