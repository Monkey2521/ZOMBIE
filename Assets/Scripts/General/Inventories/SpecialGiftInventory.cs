using System;

using UnityEngine;

using ZombieSurvival.Rewards;
using ZombieSurvival.UI;

using Zenject;

namespace ZombieSurvival.General.Inventories
{
    public sealed class SpecialGiftInventory : Inventory
    {
        [Header("SpecialGift inventory settings")]
        [Tooltip("Cooldown in seconds")]
        [SerializeField] private int _giftCooldown;

        private DateTime _giftGainedTime;

        [Inject] MainMenu _mainMenu;

        public override bool Add(ConcreteReward reward)
        {
            if (reward is SpecialGiftReward giftReward)
            {
                bool onCooldown = (DateTime.Now - _giftGainedTime).TotalSeconds < _giftCooldown;
                int cooldownTime = (int)(_giftCooldown - (DateTime.Now - _giftGainedTime).TotalSeconds);

                if (!onCooldown)
                {
                    _giftGainedTime = DateTime.Now;

                    OnInventoryChanged();

                    _mainMenu.ShowRewards(giftReward.Reward);

                    return true;
                }
                else
                {
                    _mainMenu.ShowPopupMessage("Gift on cooldown for " + IntegerFormatter.GetTime(cooldownTime));
                }
            }

            return false;
        }

        public override bool IsEnough(ConcreteReward reward) => false;

        public override bool Spend(ConcreteReward unreward) => false;

        #region Serialization
        protected override InventoryData GetData()
        {
            GiftData data = new GiftData();

            data.time = _giftGainedTime;

            return data;
        }

        public override bool LoadData()
        {
            if (!base.LoadData())
            {
                _giftGainedTime = DateTime.Now;

                OnInventoryChanged();

                return false;
            }

            return true;
        }

        protected override bool UnpackData(InventoryData data)
        {
            if (data is GiftData giftData)
            {
                _giftGainedTime = giftData.time;

                return true;
            }

            return false;
        }

        [Serializable]
        private class GiftData : InventoryData
        {
            public DateTime time;
        }
        #endregion
    }
}