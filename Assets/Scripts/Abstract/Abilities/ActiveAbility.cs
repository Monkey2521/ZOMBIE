using UnityEngine;
using ZombieSurvival.Interfaces;

namespace ZombieSurvival.Abilities
{
    public abstract class ActiveAbility : AbilityContainer, IUpdatable
    {
        protected float _attackIntervalTimer;
        protected bool _isReady;

        /// <summary>
        /// Update cooldown timer
        /// </summary>
        public virtual void OnUpdate()
        {
            if (_isReady) return;

            _attackIntervalTimer -= Time.deltaTime;

            if (_attackIntervalTimer <= 0f)
            {
                _isReady = true;
            }
        }
    }
}
