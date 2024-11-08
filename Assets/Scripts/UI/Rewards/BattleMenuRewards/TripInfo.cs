using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.General.Inventories;
using ZombieSurvival.Rewards;
using ZombieSurvival.Rewards.UI;
using ZombieSurvival.UI;

using Zenject;

public class TripInfo : UIMenu
{
    [Header("TripInfo settings")]
    [SerializeField] private TripForSupplies _supplies;
    [SerializeField] private TripForSuppliesInventory _inventory;
    [SerializeField] private RewardsInfo _rewardsInfo;
    [SerializeField] private GridLayoutGroup _rewardsGrid;
    [SerializeField] private RectTransform _rewardsGridTransform;

    [Space(5)]
    [SerializeField] private CurrencyData _goldData;
    [SerializeField] private Text _goldPerTickText;
    [SerializeField] private Text _expPerTickText;

    [SerializeField] private Text _timerText;

    [Inject] private MainInventory _mainInventory;

    public override void Display(bool playAnimation = false)
    {
        base.Display(playAnimation);

        _rewardsInfo.Display();

        TripForSuppliesRewardData rewardData = _supplies.GetCurrentRewards();

        CurrencyReward goldReward = rewardData.GoldPerTick.Find(item => item.Currency.CurrencyData.Equals(_goldData));
        ExpirienceReward expReward = rewardData.ExpPerTick.Find(item => item is ExpirienceReward);

        _goldPerTickText.text = (3600f / rewardData.TickTime * (goldReward != null ? goldReward.Amount : 0)) + "/h";
        _expPerTickText.text = (3600f / rewardData.TickTime * (expReward != null ? expReward.Amount : 0)) + "/h";

        List<ConcreteReward> rewards = GetCurrentRewards(rewardData);

        if (rewards == null) return;

        _rewardsInfo.ShowReward(rewards, playSound: false, addToInventory: false);
    }

    public override void Hide(bool playAnimation = false)
    {
        base.Hide(playAnimation);

        _rewardsInfo.Hide();
    }

    private void FixedUpdate()
    {
        if (!_enabled) return;
        
        int time = (int)(DateTime.Now - _inventory.LastRewardTime).TotalSeconds;

        _timerText.text = IntegerFormatter.GetTime(time);
    }
    public void OnObtainClick()
    {
        _mainInventory.Add(new TripForSuppliesReward(_supplies.GetCurrentRewards()));
    }

    private List<ConcreteReward> GetCurrentRewards(TripForSuppliesRewardData rewardData)
    {
        if (rewardData == null)
        {
            return null;
        }

        List<ConcreteReward> rewards = new List<ConcreteReward>();

        DateTime currentTime;
        DateTime maxTime;
        DateTime now = DateTime.Now;

        #region Gold & Exp
        currentTime = _inventory.LastRewardTime.AddSeconds(rewardData.TickTime);
        maxTime = _inventory.LastRewardTime.AddSeconds(_inventory.TripTimeLimit);

        while (currentTime <= maxTime && currentTime <= now)
        {
            rewards.AddRange(rewardData.GoldPerTick);
            rewards.AddRange(rewardData.ExpPerTick);

            currentTime = currentTime.AddSeconds(rewardData.TickTime);
        }
        #endregion

        #region EquipmentMaterials
        if (rewardData.MaterialReward != null && rewardData.MaterialReward.Count > 0)
        {
            currentTime = _inventory.LastMaterialRewardTime.AddSeconds(rewardData.RequiredTimeForMaterial);
            maxTime = _inventory.LastMaterialRewardTime.AddSeconds(_inventory.TripTimeLimit);

            while (currentTime <= maxTime && currentTime <= now)
            {
                rewards.AddRange(rewardData.MaterialReward);

                currentTime = currentTime.AddSeconds(rewardData.RequiredTimeForMaterial);
            }
        }
        #endregion

        #region Equipment
        if (rewardData.EquipmentReward != null && rewardData.EquipmentReward.Count > 0)
        {
            currentTime = _inventory.LastEquipmentRewardTime.AddSeconds(rewardData.RequiredTimeForEquipment);
            maxTime = _inventory.LastEquipmentRewardTime.AddSeconds(_inventory.TripTimeLimit);

            while (currentTime <= maxTime && currentTime <= now)
            {
                rewards.AddRange(rewardData.EquipmentReward);

                currentTime = currentTime.AddSeconds(rewardData.RequiredTimeForEquipment);
            }
        }
        #endregion

        return rewards;
    }
}
