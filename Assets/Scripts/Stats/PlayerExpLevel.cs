using UnityEngine;

using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.General.Enums;
using ZombieSurvival.General.Sounds;
using ZombieSurvival.UI;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public class PlayerExpLevel : Stat
    {
        [SerializeField] protected Expirience _exp;

        [Header("LevelUp settings")]
        [SerializeField] protected SoundList _sounds;
        [SerializeField] protected int _baseExpForLevelUp;
        [Tooltip("ExpForLevel = BaseExpForLevel * LevelMultiplier * CurrentLevel")]
        [SerializeField] protected float _levelMultiplier;
        [SerializeField] protected PlayerExpBar _expBar;

        protected float _totalExp;

        public Expirience Exp => _exp;
        /// <summary>
        /// Expirience value required for level up
        /// </summary>
        public int ExpForLevel => (int)(_baseExpForLevelUp * _levelMultiplier * _value);
        /// <summary>
        /// Can be in range [0, 1]
        /// </summary>
        public float LevelProgress => _exp.Value / ExpForLevel;
        public float TotalExp => _totalExp;

        public override void Initialize()
        {
            base.Initialize();

            _exp.SetValue(0);

            _totalExp = 0;

            if (_expBar != null)
            {
                _expBar.Initialize(this);
            }

            UpdateCounter();
        }

        public PlayerExpLevel(StatData statData, int baseExpForLevel, float levelMuliplier, 
                              ExpirienceData expirienceData, PlayerExpBar expBar = null,
                              UpgradeList upgradeList = null, bool isDebug = false) : 
            base(statData, upgradeList, isDebug)
        {
            _baseExpForLevelUp = baseExpForLevel;
            _levelMultiplier = levelMuliplier;

            _exp = new Expirience(expirienceData);
            _expBar = expBar;
        }

        /// <summary>
        /// Add expirience to player
        /// </summary>
        /// <param name="exp">Value need to add</param>
        public void AddExp(Expirience expirience)
        {
            float addedExp = (expirience.Value + _upgrades.UpgradesValue) * _upgrades.UpgradesMultiplier;

            _totalExp += addedExp;
#if DEBUG
            if (_isDebug) Debug.Log("Add " + expirience.Value + " expirience. Total: " + addedExp);
#endif
            _exp.Add(addedExp);

            if (LevelProgress >= 1)
            {
                _exp.SetValue(0);
                _value++;

                _sounds.PlaySound(SoundTypes.LevelUp);

                EventBus.Publish<IPlayerLevelUpHandler>(handler => handler.OnPlayerLevelUp());
            }

            UpdateCounter();
        }

        protected void UpdateCounter()
        {
            if (_expBar != null)
            {
                _expBar.UpdateLevel();
                _expBar.UpdateExp();
            }
#if DEBUG
            else if (_isDebug) Debug.Log("Missing ExpBar!");
#endif
        }

        #region Serialization
        public SerializableData SaveData()
        {
            ExpLevelData data = new ExpLevelData();

            data.level = (int)_value;
            data.expirience = (int)_exp.Value;
            data.totalExp = (int)_totalExp;

            return data;
        }

        public bool LoadData(SerializableData data)
        {
            if (data == null)
            {
#if DEBUG
                if (_isDebug) Debug.Log("PlayerLevel: Missing data!");
#endif
                return false;
            }

            if (data is ExpLevelData levelData)
            {
                SetValue(levelData.level);
                _exp.SetValue(levelData.expirience);
                _totalExp = levelData.totalExp;

                UpdateCounter();
#if DEBUG
                if (_isDebug) Debug.Log("PlayerLevel: Load data");
#endif
                return true;
            }
#if DEBUG
            if (_isDebug) Debug.Log("PlayerLevel: Data type error!");
#endif
            return false;
        }

        public void ResetData()
        {
            _exp.SetValue();
            _value = BaseValue;
            _totalExp = 0;

            UpdateCounter();
        }

        [System.Serializable]
        public class ExpLevelData : SerializableData
        {
            public int level;
            public int expirience;
            public int totalExp;
        }
        #endregion
    }
}