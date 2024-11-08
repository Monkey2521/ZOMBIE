using UnityEngine;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public sealed class PlayerStats : CharacterStats
    {
        [Header("Player stats")]
        [SerializeField] private PlayerExpLevel _expLevel;
        [SerializeField] private PickUpRadius _pickUpRadius;
        [SerializeField] private ReanimationCount _rebornCount;

        [Space(5)]
        [SerializeField] private AbilityChooseCount _abilityChooseCount;
        [SerializeField] private AbilitiesPerChoice _abilitiesPerChoice;
        [SerializeField] private AbilitiesRerollCount _abilitiesRerollCount;

        public PlayerExpLevel ExpLevel => _expLevel;
        public PickUpRadius PickUpRadius => _pickUpRadius;
        public ReanimationCount RebornCount => _rebornCount;
        public AbilityChooseCount AbilityChooseCount => _abilityChooseCount;
        public AbilitiesPerChoice AbilitiesPerChoice => _abilitiesPerChoice;
        public AbilitiesRerollCount AbilitiiesRerollCount => _abilitiesRerollCount;

        public override void Initialize()
        {
            base.Initialize();

            _expLevel.Initialize();
            _pickUpRadius.Initialize();
            _rebornCount.Initialize();

            _abilityChooseCount.Initialize();
            _abilitiesPerChoice.Initialize();
            _abilitiesRerollCount.Initialize();
        }

        public override void GetUpgrade(Upgrade upgrade)
        {
            base.GetUpgrade(upgrade);

            _expLevel.Upgrade(upgrade);
            _pickUpRadius.Upgrade(upgrade);
            _rebornCount.Upgrade(upgrade);

            _abilityChooseCount.Upgrade(upgrade);
            _abilitiesPerChoice.Upgrade(upgrade);
            _abilitiesRerollCount.Upgrade(upgrade);
        }

        public override void DispelUpgrade(Upgrade upgrade)
        {
            base.DispelUpgrade(upgrade);

            _expLevel.DispelUpgrade(upgrade);
            _pickUpRadius.DispelUpgrade(upgrade);
            _rebornCount.DispelUpgrade(upgrade);

            _abilityChooseCount.DispelUpgrade(upgrade);
            _abilitiesPerChoice.DispelUpgrade(upgrade);
            _abilitiesRerollCount.DispelUpgrade(upgrade);
        }
    }
}