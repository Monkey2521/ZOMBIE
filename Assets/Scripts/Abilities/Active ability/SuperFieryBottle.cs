using UnityEngine;

namespace ZombieSurvival.Abilities
{
    public class SuperFieryBottle : FieryBottle
    {
        protected override Vector3 GetProjectileMoveDirection()
        {
            Vector3 pos = transform.position;

            return new Vector3(pos.x, 0f, pos.z) + GetDeltaMoveDirection() * _stats.AttackRange.Value;
        }

        protected override Vector3 GetDeltaMoveDirection()
        {
            float delta = 2 * Mathf.PI / _stats.ProjectileNumber.Value;

            return new Vector3
                (
                    Mathf.Cos(delta * _spawnCount) * _scatterMultiplier,
                    0,
                    Mathf.Sin(delta * _spawnCount) * _scatterMultiplier
                ).normalized;
        }
    }
}