using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Interfaces;
using ZombieSurvival.Objects;
using ZombieSurvival.Stats;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Characters
{
    public abstract class CameraTransformFollow : ZSMonoBehaviour, IUpgradable, IFixedUpdatable
    {
        [Header("Camera follow settings")]
        [SerializeField] private Transform _transformToFollow;
        [SerializeField] private Transform _followedTransform;

        [SerializeField] private Vector3 _cameraPos;

        public Vector3 CameraDeltaPos => _cameraPos * Stats.CameraPosRange.Value;
        public UpgradeList Upgrades => Stats.CameraPosRange.Upgrades;
        public abstract CameraFollowStats Stats { get; }

        protected virtual void OnEnable()
        {
            Stats.Initialize();
        }

        public virtual void OnFixedUpdate()
        {
            _followedTransform.position = _transformToFollow.position + _cameraPos * Stats.CameraPosRange.Value;
            _followedTransform.LookAt(_transformToFollow);
        }

        public virtual bool Upgrade(Upgrade upgrade)
        {
            Stats.GetUpgrade(upgrade);

            return true;
        }
    }
}