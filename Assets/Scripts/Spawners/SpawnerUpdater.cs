using System.Collections.Generic;

using UnityEngine;

namespace ZombieSurvival.Spawners
{
    public sealed class SpawnerUpdater : MonoBehaviour
    {
        [SerializeField] private List<Spawner> _spawners;

        private void Update()
        {
            foreach (Spawner spawner in _spawners)
            {
                spawner.OnUpdate();
            }
        }

        private void FixedUpdate()
        {
            foreach (Spawner spawner in _spawners)
            {
                spawner.OnFixedUpdate();
            }
        }
    }
}