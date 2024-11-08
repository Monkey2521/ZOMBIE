using UnityEngine;

using ZombieSurvival.General;

namespace ZombieSurvival.UI.Shop
{
    public abstract class LinearRoulette : Roulette
    {
        [Header("Linear roulette settings")]
        [SerializeField] protected Vector3 _rouletteStartPosition;

        protected override RouletteWindow InitializeWindow(RouletteWindow window)
        {
            window.Initialize
                    (
                        Random.Range(_minStartSpeed, _maxStartSpeed),
                        Random.Range(_speedMinDecreasePerFrame, _speedMaxDecreasePerFrame),
                        Random.Range(_speedMinDecreaseMultiplier, _speedMaxDecreaseMultiplier),
                        _rouletteStartPosition,
                        Vector3.left
                    );

            return window;
        }

        protected override void InitializeSlots<TChance>(ChanceCombiner<TChance> chances, RouletteWindow window)
        {
            for (int j = 0; j < _maxSlotsCount / ScreenScaler.MinDelta; j++)
            {
                RouletteSlot slot = _slotsPool.Pull();

                slot.transform.SetParent(window.SlotsParent);

                slot.Initialize(GetConcreteReward(chances));

                _slots.Add(slot);
            }
        }
    }
}