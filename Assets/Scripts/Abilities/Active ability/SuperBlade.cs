using UnityEngine;

namespace ZombieSurvival.Abilities
{
    public class SuperBlade : Blade
    {
        /// <summary>
        /// Returns Vector3.zero
        /// </summary>
        /// <returns></returns>
        protected override Vector3 GetProjectileMoveDirection()
        {
            return Vector3.zero;
        }
    }
}