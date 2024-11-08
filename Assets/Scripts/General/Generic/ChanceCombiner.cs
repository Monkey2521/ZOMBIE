using System;
using System.Collections.Generic;

using UnityEngine;

namespace ZombieSurvival.General
{
    [System.Serializable]
    public partial class ChanceCombiner<TObject> where TObject : class
    {
        [SerializeField] private List<ObjectChanceSpawn<TObject>> _spawnChances;

        public List<ObjectChanceSpawn<TObject>> SpawnChances => _spawnChances;

        public ChanceCombiner(List<ObjectChanceSpawn<TObject>> spawnChances)
        {
            _spawnChances = new List<ObjectChanceSpawn<TObject>>();

            foreach (ObjectChanceSpawn<TObject> spawnChance in spawnChances)
            {
                Add(new ObjectChanceSpawn<TObject>(spawnChance));
            }
        }

        public ChanceCombiner()
        {
            _spawnChances = new List<ObjectChanceSpawn<TObject>>();
        }

        public void Initialize()
        {
            foreach (ObjectChanceSpawn<TObject> spawnChance in _spawnChances)
            {
                spawnChance.SpawnChance.Initialize();
            }
        }

        public ChanceCombiner<TObject> Clone() => new ChanceCombiner<TObject>(_spawnChances);

        /// <summary>
        /// Add spawnChance in all chances sorted by chance probability
        /// </summary>
        /// <param name="spawnChance"></param>
        /// <param name="sortByProbability">If true add spawnChance with sorting</param>
        public void Add(ObjectChanceSpawn<TObject> spawnChance, bool sortByProbability = true)
        {
            spawnChance.SpawnChance.Initialize();

            if (_spawnChances.Count == 0 || !sortByProbability)
            {
                _spawnChances.Add(spawnChance);

                return;
            }

            for (int i = 0; i < _spawnChances.Count; i++)
            {
                if (_spawnChances[i].SpawnChance.Probability > spawnChance.SpawnChance.Probability)
                {
                    if (i != _spawnChances.Count - 1 && _spawnChances[i + 1].SpawnChance.Probability <= spawnChance.SpawnChance.Probability)
                    {
                        _spawnChances.Insert(i + 1, spawnChance);
                        return;
                    }
                    else if (i == _spawnChances.Count - 1)
                    {
                        _spawnChances.Add(spawnChance);
                        return;
                    }
                }
                else if (_spawnChances[i].SpawnChance.Probability <= spawnChance.SpawnChance.Probability)
                {
                    if (i > 0)
                    {
                        _spawnChances.Insert(i - 1, spawnChance);
                        return;
                    }
                    else
                    {
                        _spawnChances.Insert(0, spawnChance);
                        return;
                    }
                }
            }
        }

        public bool Remove(ObjectChanceSpawn<TObject> spawnChance) => _spawnChances.Remove(spawnChance);

        /// <summary>
        /// Calculate chances and find striked object
        /// </summary>
        /// <returns>Returns first first object with TObjectrueChance or last striked SpawnChance</returns>
        public TObject GetStrikedObject()
        {
            if (_spawnChances == null || _spawnChances.Count == 0) return null;

            TObject strikedObject = _spawnChances[0].Object;

            foreach (ObjectChanceSpawn<TObject> spawnChance in _spawnChances)
            {
                if (spawnChance.ChanceIsTrue) return spawnChance.Object;

                if (spawnChance.SpawnChance.IsStrike)
                {
                    strikedObject = spawnChance.Object;
                }
            }

            return strikedObject;
        }
    }
}