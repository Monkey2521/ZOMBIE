using UnityEngine;

using ZombieSurvival.Stats;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Objects
{
    [System.Serializable]
    public sealed class PlayerCameraStats : CameraFollowStats
    {
        [SerializeField] private Duration _shakeDuration;
        [SerializeField] private Duration _bossAttackShakeDuration;
        [SerializeField] private Duration _shakeDirectionMoveDuration;
        [SerializeField] private MoveSpeed _shakeSpeed;

        public Duration ShakeDuration => _shakeDuration;
        public Duration BossAttackShakeDuration => _bossAttackShakeDuration;
        public Duration ShakeDirectionMoveDuration => _shakeDirectionMoveDuration;
        public MoveSpeed ShakeSpeed => _shakeSpeed;

        public override void Initialize()
        {
            base.Initialize();

            _shakeDuration.Initialize();
            _bossAttackShakeDuration.Initialize();
            _shakeDirectionMoveDuration.Initialize();
            _shakeSpeed.Initialize();
        }

        public override void GetUpgrade(Upgrade upgrade)
        {
            base.GetUpgrade(upgrade);

            _shakeDuration.Upgrade(upgrade);
            _bossAttackShakeDuration.Upgrade(upgrade);
            _shakeDirectionMoveDuration.Upgrade(upgrade);
            _shakeSpeed.Upgrade(upgrade);
        }

        public override void DispelUpgrade(Upgrade upgrade)
        {
            base.DispelUpgrade(upgrade);

            _shakeDuration.DispelUpgrade(upgrade);
            _bossAttackShakeDuration.DispelUpgrade(upgrade);
            _shakeDirectionMoveDuration.DispelUpgrade(upgrade);
            _shakeSpeed.DispelUpgrade(upgrade);
        }
    }
}