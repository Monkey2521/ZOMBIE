using UnityEngine;
using static UnityEngine.Mathf;

namespace ZombieSurvival.Abilities
{
    public class SuperShotgun : Shotgun 
    {
        protected readonly float _shootsOffset = PI * 0.5f;
        protected override Vector3 GetProjectileMoveDirection()
        {
            return GetDeltaMoveDirection();
        }

        protected override Vector3 GetDeltaMoveDirection()
        {
            float delta = 2 * PI / _stats.ProjectileNumber.Value;

            return transform.TransformDirection(new Vector3
                (
                    Cos(_shootsOffset + delta * (_spawnCount)) * _scatterMultiplier,
                    0,
                    Sin(_shootsOffset + delta * (_spawnCount)) * _scatterMultiplier
                ).normalized);
        }
    }
}