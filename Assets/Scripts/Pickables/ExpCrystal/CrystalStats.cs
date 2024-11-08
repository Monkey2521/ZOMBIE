using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Objects.Pickables;

namespace ZombieSurvival.Stats
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Level/Crystal spawning stats", fileName = "New crystal stats")]
    public class CrystalStats : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private List<ObjectChanceSpawn<CrystalParam>> _crystalSpawnParams;

        public List<ObjectChanceSpawn<CrystalParam>> CrystalSpawnParams => _crystalSpawnParams;
    }
}