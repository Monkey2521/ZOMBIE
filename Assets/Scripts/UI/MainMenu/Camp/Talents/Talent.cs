using UnityEngine;

using ZombieSurvival.UI.Camp;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.General
{
    [System.Serializable]
    public class Talent
    {
        [SerializeField] private Sprite _lockedIcon;
        [SerializeField] private Sprite _unlockedIcon;
        [SerializeField] private Sprite _lockedBackground;
        [SerializeField] private Sprite _unlockedBackground;

        [Space(5)]
        [SerializeField] private Upgrade _upgrade;
        [SerializeField] private string _description;
        [SerializeField] private int _requiredLevel;
        [SerializeField] private Currency _requiredCurrency;

        private TalentSlot _slot;

        private bool _unlocked;

        public Sprite LockedIcon => _lockedIcon;
        public Sprite UnlockedIcon => _unlockedIcon;
        public Sprite UnlockedBackground => _unlockedBackground;
        public Sprite LockedBackground => _lockedBackground;
        public string Description => _description;
        public Upgrade Upgrade => _upgrade;
        public int RequiredLevel => _requiredLevel;
        public Currency RequiredCurrency => _requiredCurrency;
        public bool Unlocked => _unlocked;

        public void Initialize(bool unlocked = false)
        {
            _unlocked = unlocked;
        }

        public void SetSlot(TalentSlot slot)
        {
            _slot = slot;

            _slot?.UpdateSlot();
        }

        public void Unlock()
        {
            _unlocked = true;

            _slot?.UpdateSlot();
        }
    }
}