using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Rewards;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.General.Inventories
{
    public sealed class CampInventory : Inventory
    {
        [Header("CampInventory settings")]
        [SerializeField] private List<CampUpgrade> _campUpgrades;
        [SerializeField] private List<Talent> _talents;

        public List<Upgrade> CampUpgrades
        {
            get
            {
                List<Upgrade> upgrades = new List<Upgrade>();

                foreach (var upgrade in _campUpgrades)
                {
                    Upgrade current = upgrade.CurrentUpgrade;

                    if (current != null)
                    {
                        upgrades.Add(current);
                    }
                }

                foreach (var talent in _talents.FindAll(item => item.Unlocked == true))
                {
                    Upgrade current = talent.Upgrade;

                    if (current != null)
                    {
                        upgrades.Add(current);
                    }
                }

                return upgrades;
            }
        }

        /// <summary>
        /// All available camp upgrades
        /// </summary>
        public List<CampUpgrade> Upgrades => _campUpgrades;
        /// <summary>
        /// All available camp talents
        /// </summary>
        public List<Talent> Talents => _talents;

        public override void Initialize(MainInventory mainInventory = null)
        {
            base.Initialize(mainInventory);

            foreach (CampUpgrade upgrade in _campUpgrades)
            {
                upgrade.Initialize();
            }

            foreach(Talent talent in _talents)
            {
                talent.Initialize();
            }
        }

        public override bool Add(ConcreteReward reward)
        {
            if (reward is CampTalentReward talentReward)
            {
                Talent talent = _talents.Find(item => item.Equals(talentReward.Talent));

                if (talent != null)
                {
                    talent.Unlock();

                    OnInventoryChanged();

                    return true;
                }
            }
            else if (reward is CampUpgradeReward upgradeReward)
            {
                CampUpgrade upgrade = _campUpgrades.Find(item => item.Equals(upgradeReward.ChanceData.Building.CampUpgrade));

                if (upgrade != null)
                {
                    upgrade.Upgrade();

                    OnInventoryChanged();

                    return true;
                }
            }

            return false;
        }

        public override bool Spend(ConcreteReward unreward) => false;

        public override bool IsEnough(ConcreteReward reward) => false;

        #region Serialization
        protected override InventoryData GetData()
        {
            CampUpgradeData data = new CampUpgradeData();

            foreach (var upgrade in _campUpgrades)
            {
                data.Add(upgrade);
            }

            foreach (var talent in _talents)
            {
                data.Add(talent);
            }

            return data;
        }

        protected override bool UnpackData(InventoryData data)
        {
            if (data == null) return false;

            if (data is CampUpgradeData loadedData)
            {
                if (loadedData.upgrades.Count != _campUpgrades.Count)
                {
#if DEBUG
                    if (_isDebug) Debug.Log("Camp inventory upgrades load error!");
#endif
                }
                else
                {
                    for (int i = 0; i < _campUpgrades.Count; i++)
                    {
                        _campUpgrades[i].Level.SetValue(loadedData.upgrades[i].level);
                        _campUpgrades[i].UpdateValues();
                    }
                }

                if (loadedData.talents.Count != _talents.Count)
                {
#if DEBUG
                    if (_isDebug) Debug.Log("Camp inventory talents load error!");
#endif

                }
                else
                {
                    for (int i = 0; i < _talents.Count; i++)
                    {
                        _talents[i].Initialize(loadedData.talents[i].unlocked);
                    }
                }

                return true;
            }
            else return false;
        }

        public override void ResetData()
        {
            base.ResetData();

            Initialize();
        }

        [System.Serializable]
        private class CampUpgradeData : InventoryData
        {
            public List<ConcreteUpgradeData> upgrades;
            public List<ConcreteTalentData> talents;

            public CampUpgradeData()
            {
                upgrades = new List<ConcreteUpgradeData>();
                talents = new List<ConcreteTalentData>();
            }

            public void Add(CampUpgrade upgrade)
            {
                ConcreteUpgradeData data = new ConcreteUpgradeData();

                data.level = (int)upgrade.Level.Value;

                upgrades.Add(data);
            }

            public void Add(Talent talent)
            {
                ConcreteTalentData data = new ConcreteTalentData();

                data.unlocked = talent.Unlocked;

                talents.Add(data);
            }

            [System.Serializable]
            public class ConcreteUpgradeData
            {
                public int level;
            }

            [System.Serializable]
            public class ConcreteTalentData
            {
                public bool unlocked;
            }
        }
        #endregion
    }
}