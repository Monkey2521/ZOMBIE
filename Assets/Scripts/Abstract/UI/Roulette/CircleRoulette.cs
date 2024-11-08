using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;

namespace ZombieSurvival.UI.Shop
{
    public abstract class CircleRoulette : Roulette
    {
        [Header("Circle roulette settings")]
        [Tooltip("Shake chances sequence")]
        [SerializeField] protected bool _shakeChances = true;
        [SerializeField][Range(10, 100)] protected int _maxVerticesPerRoulette = 50;
        [SerializeField][Range(0f, 360f)] protected float _minSegmentAngleOffset = 0f;
        [SerializeField][Range(0f, 360f)] protected float _maxSegmentAngleOffset = 180f;

        [Space(5)]
        [SerializeField] protected GridLayoutGroup _windowsGrid;
        [SerializeField][Range(0f, 1f)] protected float _segmentsSize = 0.95f;

        protected float _firstSegmentAngleOffset = 0f;


        public override void DisplayRoulette<TChance>(ChanceCombiner<TChance> chances, int roulettesCount = 1, bool autoStart = true)
        {
            _firstSegmentAngleOffset = Random.Range(_minSegmentAngleOffset, _maxSegmentAngleOffset);

            base.DisplayRoulette(chances, roulettesCount, autoStart);
        }

        protected override RouletteWindow InitializeWindow(RouletteWindow window)
        {
            window.Initialize
                    (
                        Random.Range(_minStartSpeed, _maxStartSpeed),
                        Random.Range(_speedMinDecreasePerFrame, _speedMaxDecreasePerFrame),
                        Random.Range(_speedMinDecreaseMultiplier, _speedMaxDecreaseMultiplier),
                        Vector3.zero,
                        Vector3.left
                    );

            return window;
        }

        protected override void InitializeSlots<TChance>(ChanceCombiner<TChance> chances, RouletteWindow window)
        {
            ChanceCombiner<TChance> newChances = _shakeChances ? GetShakedChances(chances) : chances;

            for (int i = 0; i < newChances.SpawnChances.Count; i++)
            {
                RouletteSlot slot = _slotsPool.Pull();

                slot.transform.SetParent(window.SlotsParent);

                slot.Initialize(GetConcreteReward(newChances, concreteChanceIndex: i));

                _slots.Add(slot);
            }
        }

        protected virtual ChanceCombiner<TObject> GetShakedChances<TObject>(ChanceCombiner<TObject> chances)
            where TObject : class
        {
            ChanceCombiner<TObject> newChances = new ChanceCombiner<TObject>();

            int chanceCount = chances.SpawnChances.Count;

            for (int i = 0; i < chanceCount; i++)
            {
                ObjectChanceSpawn<TObject> randomChance;

                do
                {
                    randomChance = chances.SpawnChances[Random.Range(0, chanceCount)];

                } while (newChances.SpawnChances.Contains(randomChance));

                newChances.Add(randomChance, sortByProbability: false);
            }

            return newChances;
        }

        protected virtual CircleRouletteSlotData GetSlotData<TObject>(List<ObjectChanceSpawn<TObject>> chances, int chanceIndex)
            where TObject : class
        {
            float startAngle = _firstSegmentAngleOffset;

            for (int i = 0; i < chanceIndex; i++)
            {
                startAngle += 360f * chances[i].SpawnChance.Probability;
            }

            return new CircleRouletteSlotData
                    (
                        GetSegmentSprite(chances, chanceIndex),
                        (int)(_maxVerticesPerRoulette * chances[chanceIndex].SpawnChance.Probability),
                        startAngle,
                        360f * chances[chanceIndex].SpawnChance.Probability,
                        _windowsGrid.cellSize.x * _segmentsSize * 0.5f,
                        IsFillSegmentByAngle(chances, chanceIndex)
                    );
        }

        protected abstract Sprite GetSegmentSprite<TObject>(List<ObjectChanceSpawn<TObject>> chances, int chanceIndex)
            where TObject : class;

        protected abstract bool IsFillSegmentByAngle<TObject>(List<ObjectChanceSpawn<TObject>> chances, int chanceIndex)
            where TObject : class;
    }
}