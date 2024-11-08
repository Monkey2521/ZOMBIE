using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Abilities;
using ZombieSurvival.Spawners;

namespace ZombieSurvival.Characters
{
    public class BossZombie : Enemy
    {
        [SerializeField] private List<Weapon> _additionalAbilities;

        private BossSpawner _spawner;

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < _additionalAbilities.Count; i++)
            {
                GetAbility(_additionalAbilities[i]);
            }
        }

        public void InitializeSpawner(BossSpawner spawner)
        {
            _spawner = spawner;
        }

        protected override void OnDie(bool instantly = false)
        {
            _spawner.OnBossDies(transform.position);

            base.OnDie(instantly);
        }
    }
}