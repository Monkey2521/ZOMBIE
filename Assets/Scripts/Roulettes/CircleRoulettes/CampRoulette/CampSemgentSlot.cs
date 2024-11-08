using UnityEngine;

using ZombieSurvival.Rewards;

namespace ZombieSurvival.UI.Shop
{
    public class CampSemgentSlot : CircleSegmentSlot
    {
        protected override void InitializeSlot()
        {
            if (_reward is CampUpgradeReward reward)
            {
                base.InitializeSlot();

                _rewardIconImage.sprite = reward.ChanceData.Building.CampUpgrade.Unlocked ?
                    reward.ChanceData.Building.CampUpgrade.UnlockedIcon : reward.ChanceData.Building.CampUpgrade.LockedIcon;
            }
            else
            {
#if DEBUG
                if (_isDebug) Debug.Log(name + ": Reward type error!");
#endif
            }
        }
    }
}