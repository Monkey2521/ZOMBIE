using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Achievements;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.General.Inventories
{
    public class AchievementInventory : Inventory
    {
        [Header("AchievementInventory settings")]
        [SerializeField] private AchievementsList _achievementsList;
        [SerializeField] private Transform _achievementsParent;

        private List<Achievement> _achievements;

        public List<Achievement> Achievements => _achievements;

        public override void Initialize(MainInventory mainInventory = null)
        {
            base.Initialize(mainInventory);

            _achievements = new List<Achievement>();

            foreach (Achievement ach in _achievementsList.Achievements)
            {
                Achievement achievement = Instantiate(ach, _achievementsParent);
                achievement.Initialize(this);

                _achievements.Add(achievement);
            }
        }

        public override bool Add(ConcreteReward reward)
        {
            if (reward is AchievementObtainReward achievementReward)
            {
                Achievement achievement = _achievements.Find(item => item.Equals(achievementReward.Achievement));

                if (achievement != null)
                {
                    achievement.Claim();

                    OnInventoryChanged();

                    return true;
                }
            }

            return false;
        }

        public override bool Spend(ConcreteReward unreward) => false;

        public override bool IsEnough(ConcreteReward reward) => false;

        public void OnAchievementValueChanged(Achievement achievement)
        {
            OnInventoryChanged();

            // TODO better saving
        }

        #region Serialization
        protected override InventoryData GetData()
        {
            AchievementInventoryData data = new AchievementInventoryData();

            foreach (Achievement achievement in _achievements)
            {
                data.AddAchievement(achievement);
            }

            return data;
        }

        protected override bool UnpackData(InventoryData data)
        {
            if (data == null) return false;

            if (data is AchievementInventoryData loadedData)
            {
                if (loadedData.achievements.Count > _achievements.Count)
                {
#if DEBUG
                    if (_isDebug) Debug.Log("Loading data error! Have missing achievements");
#endif
                    return false;
                }

                for (int i = 0; i < loadedData.achievements.Count; i++)
                {
                    _achievements[i].LoadData(loadedData.achievements[i]);
                }

                return true;
            }

            return false;
        }

        public override void ResetData()
        {
            base.ResetData();

            if (_achievements == null) return;

            foreach (Achievement achievement in _achievements)
            {
                achievement.Initialize(this);
            }
        }

        [System.Serializable]
        private class AchievementInventoryData : InventoryData
        {
            public List<ConcreteAchievementData> achievements;

            public AchievementInventoryData()
            {
                achievements = new List<ConcreteAchievementData>();
            }

            public void AddAchievement(Achievement achievement)
            {
                achievements.Add(new ConcreteAchievementData(achievement));
            }
        }
        #endregion
    }
}