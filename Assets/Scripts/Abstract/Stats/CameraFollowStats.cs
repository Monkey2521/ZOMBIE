using UnityEngine;

using ZombieSurvival.Interfaces;
using ZombieSurvival.Stats;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Objects
{
    [System.Serializable]
    public abstract class CameraFollowStats : IObjectStats
    {
        [SerializeField] protected Radius _cameraPosRange;

        public Radius CameraPosRange => _cameraPosRange;

        public virtual void Initialize()
        {
            _cameraPosRange.Initialize();
        }

        public virtual void GetUpgrade(Upgrade upgrade)
        {
            _cameraPosRange.Upgrade(upgrade);
        }

        public virtual void DispelUpgrade(Upgrade upgrade)
        {
            _cameraPosRange.DispelUpgrade(upgrade);
        }
    }
}