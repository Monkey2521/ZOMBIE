using UnityEngine;
using ZombieSurvival.Events;

namespace ZombieSurvival.Abilities
{
    public class RoadSignThrower : ProjectileWeapon
    {
        public override void Attack()
        {
            if (_isReady)
            {
                EventBus.Publish<IBossAttackHandler>(handler => handler.OnBossAttack());
                base.Attack();
            }
        }

        protected override Vector3 GetProjectileMoveDirection()
        {
            return _targetDetector.GetDirectionToNearestTarget() + GetDeltaMoveDirection();
        }

        protected override Vector3 GetDeltaMoveDirection()
        {
            Vector3 delta = Vector3.zero;

            if (_spawnCount == 0) return delta;

            else if (_spawnCount % 2 == 0)
            {
                delta += transform.TransformDirection(Vector3.right) * _scatterMultiplier * (_spawnCount - 1);
            }
            else
            {
                delta += transform.TransformDirection(Vector3.left) * _scatterMultiplier * _spawnCount;
            }

            return delta;
        }
    }
}