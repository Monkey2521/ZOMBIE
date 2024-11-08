using UnityEngine;
using ZombieSurvival.Characters;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.General.Breakpoints
{
    [System.Serializable]
    public class EliteZombieBreakpoint : Breakpoint
    {
        [Header("EliteZombie settings")]
        [SerializeField] protected Enemy _enemyToSpawnPrefab;
        [SerializeField] protected Upgrade _eliteZombieUpgrade;
        [SerializeField] private int _maxAbilitiesRewardCount;

        public Enemy EnemyToSpawnPrefab => _enemyToSpawnPrefab;
        public Upgrade EliteZombieUpgrade => _eliteZombieUpgrade;
        public int MaxAbilitiesRewardCount => _maxAbilitiesRewardCount;

        protected EliteZombieBreakpoint(EliteZombieBreakpoint breakpoint) : base(breakpoint)
        {
            _enemyToSpawnPrefab = breakpoint._enemyToSpawnPrefab;
            _eliteZombieUpgrade = breakpoint._eliteZombieUpgrade;
            _maxAbilitiesRewardCount = breakpoint._maxAbilitiesRewardCount;
        }

        public override Breakpoint Clone() => new EliteZombieBreakpoint(this);
    }
}