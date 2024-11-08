using UnityEngine;
using ZombieSurvival.Characters;
using ZombieSurvival.Rewards;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.General.Breakpoints
{
    [System.Serializable]
    public class BossBreakpoint : Breakpoint
    {
        [Header("Boss settings")]
        [SerializeField] protected Enemy _bossPrefab;
        [SerializeField] protected Upgrade _bossUpgrade;
        [SerializeField] protected GameObject _bossFence;

        [Space(5)]
        [SerializeField] protected bool _isFinalBoss;
        [SerializeField] private Reward _bossRewards;

        public Enemy BossPrefab => _bossPrefab;
        public Upgrade BossUpgrade => _bossUpgrade;
        public GameObject BossFence => _bossFence;
        public bool IsFinalBoss => _isFinalBoss;
        public Reward BossRewards => _bossRewards;

        protected BossBreakpoint(BossBreakpoint breakpoint) : base(breakpoint)
        {
            _bossPrefab = breakpoint._bossPrefab;
            _bossUpgrade = breakpoint._bossUpgrade;
            _bossFence = breakpoint._bossFence;
            _isFinalBoss = breakpoint._isFinalBoss;
            _bossRewards = breakpoint._bossRewards;
        }

        public override Breakpoint Clone() => new BossBreakpoint(this);
    }
}