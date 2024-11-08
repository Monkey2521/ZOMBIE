using UnityEngine;
using ZombieSurvival.Stats;

namespace ZombieSurvival.General
{
    [System.Serializable]
    public sealed class ObjectChanceSpawn<TObject> where TObject : class
    {
        [SerializeField] private TObject _object;
        [SerializeField] private Chance _spawnChance;

        public TObject Object => _object;
        public Chance SpawnChance => _spawnChance;
        public bool ChanceIsTrue => _spawnChance.Probability == 1;

        public ObjectChanceSpawn(TObject obj, Chance chance)
        {
            _object = obj;
            _spawnChance = chance;
        }
        
        public ObjectChanceSpawn(ObjectChanceSpawn<TObject> spawnChance)
        {
            _object = spawnChance._object;
            _spawnChance = spawnChance._spawnChance;
        }
    }
}