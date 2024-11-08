using UnityEngine;

using ZombieSurvival.Rewards;

namespace ZombieSurvival.General
{
    public class ExpirienceInventory : Inventory
    {
        private int _totalExp;

        public override void Initialize(MainInventory mainInventory = null)
        {
            base.Initialize(mainInventory);

            _totalExp = 0;
        }

        public override bool Add(ConcreteReward reward)
        {
            if (reward is ExpirienceReward expReward)
            {
                _totalExp += expReward.Amount;

                OnInventoryChanged();
#if DEBUG
                if (_isDebug) Debug.Log(name + ": Add expirience: = " + expReward.Amount + "; Total = " + _totalExp);
#endif
                return true;
            }

            return false;
        }

        public override bool IsEnough(ConcreteReward reward)
        {
            if (reward is ExpirienceReward expReward)
            {
                return _totalExp >= expReward.Amount;
            }

            return false;
        }

        public override bool Spend(ConcreteReward unreward)
        {
            if (unreward is ExpirienceReward expUnreward)
            {
                if (IsEnough(unreward))
                {
                    _totalExp -= expUnreward.Amount;

                    OnInventoryChanged();

                    return true;
                }
            }

            return false;
        }

        protected override InventoryData GetData()
        {
            ExpirienceInventoryData data = new ExpirienceInventoryData();

            data.total = _totalExp;

            return data;
        }

        protected override bool UnpackData(InventoryData data)
        {
            if (data is ExpirienceInventoryData expData)
            {
                _totalExp = expData.total;

                return true;
            }

            return false;
        }

        [System.Serializable]
        private class ExpirienceInventoryData : InventoryData
        {
            public int total;
        }
    }
}