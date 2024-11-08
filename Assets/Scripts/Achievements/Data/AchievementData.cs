using System;
using UnityEngine;
using ZombieSurvival.General;

namespace ZombieSurvival.Achievements
{
    [Serializable]
    public class AchievementData
    {
        [SerializeField] private int _level;
        [SerializeField] private int _requiredCount;
        [SerializeField] private Currency _rewardCurrency;

        private bool _unlocked;
        private bool _claimed;

        public int Level => _level;
        public int RequiredCount => _requiredCount;
        public bool Unlocked => _unlocked;
        public bool Claimed => _claimed;
        public Currency Reward => _rewardCurrency;

        public void Unlock()
        {
            _unlocked = true;
        }

        public void Claim()
        {
            _claimed = true;
        }
    }
}
