using UnityEngine;

namespace ZombieSurvival.Abilities
{
    public class ShellingProjectile : Projectile
    {
        private const float EPSILON = 0.125f;

        protected override void Move()
        {
            base.Move();

            if (transform.position.y - EPSILON <= 0f)
            {
                _releaseTimer = -1f;
                return;
            }
        }
    }
}