using UnityEngine;

using ZombieSurvival.Characters;
using ZombieSurvival.Stats;

namespace ZombieSurvival.Objects.Pickables
{
    [RequireComponent(typeof(SphereCollider))]
    public class ExpCrystal : PickableObject
    {
        [SerializeField] private SpriteRenderer _renderer;
        private int _expValue;

        /// <summary>
        /// Value that character get when take this crystal
        /// </summary>
        public int ExpValue => _expValue;

        /// <summary>
        /// Initializing exp value and color of this crystal & pool to release to
        /// </summary>
        /// <param name="pool">Pool to release to</param>
        /// <param name="param">Params need to set</param>
        public void Initialize(ObjectSpawner<PickableObject> pool, CrystalParam param)
        {
            base.Initialize(pool);

            _expValue = param.ExpValue;
            _renderer.sprite = param.Sprite;
        }

        public override void ResetObject()
        {
            _expValue = 0;
        }

        protected override void OnPickUp(float releaseDelay = 0)
        {
            if (_target != null)
            {
                if (_target.TryGetComponent(out Player player))
                {
                    if (_isDebug) Debug.Log("Add exp to player");

                    (player.Stats as PlayerStats).ExpLevel.AddExp(new Expirience(_expValue));
                }
            }
            else if (_isDebug) Debug.Log("Missing player!");  

            base.OnPickUp(releaseDelay);
        }
    }
}