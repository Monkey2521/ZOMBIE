using System;
using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.General.Inventories;
using ZombieSurvival.Stats;

namespace ZombieSurvival.Achievements
{
    public abstract class Achievement : ZSMonoBehaviour
    {
        [Header("Achievement settings")]
        [SerializeField] protected string _name;
        [SerializeField] protected Sprite _icon;
        [SerializeField] protected Sprite _background;
        [SerializeField] protected Level _level;
        [SerializeField] protected List<AchievementData> _achievementData;
        [SerializeField] protected bool _displayAchievement = true;

        protected int _currentProgress;
        protected AchievementInventory _inventory;

        public virtual string Name => _name;
        public Sprite Icon => _icon;
        public Sprite Background => _background;
        public int Level => (int)_level.Value;
        public int MaxLevel => (int)_level.MaxValue;
        public int Progress => _currentProgress;
        public virtual bool Unlocked => _level.Value == _level.MaxValue;
        public bool DisplayAchievement => _displayAchievement;

        public AchievementData CurrentObjective
        {
            get
            {
                if (Level == _level.MaxValue) return null;

                return _achievementData[Level];
            }
        }

        public AchievementData LastCompletedObjective
        {
            get
            {
                AchievementData objective = null;

                foreach (var obj in _achievementData)
                {
                    if (obj.Unlocked)
                    {
                        objective = obj;
                    }
                    else break;
                }

                return objective;
            }
        }

        public AchievementData UnclaimedObjective
        {
            get
            {
                foreach (var obj in _achievementData)
                {
                    if (obj.Unlocked && !obj.Claimed)
                    {
                        return obj;
                    }
                }

                return null;
            }
        }

        public AchievementData GetPreviousData(AchievementData data)
        {
            int index = _achievementData.IndexOf(data);

            if (index > 0)
            {
                return _achievementData[index - 1];
            }
            else return null;
        }

        public virtual void Initialize(AchievementInventory inventory)
        {
            _level.Initialize();

            _inventory = inventory;
        }

        protected virtual void UnlockNext()
        {
            CurrentObjective.Unlock();
            _level.LevelUp();
        }

        protected virtual void AddCounter(int count = 1)
        {
            _currentProgress += count;

            if (CurrentObjective != null)
            {
                while (_currentProgress >= CurrentObjective.RequiredCount)
                {
                    UnlockNext();

                    if (CurrentObjective == null)
                    {
                        break;
                    }
                }
            }

            _inventory.OnAchievementValueChanged(this);
        }

        public virtual void Claim()
        {
            UnclaimedObjective?.Claim();
        }

        public virtual void LoadData(ConcreteAchievementData data)
        {
            _level.SetValue(data.level);
            _currentProgress = data.progress;

            for(int i = 0; i < data.level; i++)
            {
                _achievementData[i].Unlock();
            }

            if (data.unclaimedLevel == -1)
            {
                foreach(AchievementData achievement in _achievementData)
                {
                    if (achievement.Unlocked)
                    {
                        achievement.Claim();
                    }
                }
            }
            else
            {
                for (int i = 0; i < data.unclaimedLevel; i++)
                {
                    _achievementData[i].Claim();
                }
            }
        }
#if DEBUG
        [ContextMenu("Unlock")]
        protected void Unlock()
        {
            AddCounter(9999999);
        }
#endif
    }
}
