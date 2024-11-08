using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.UI.Shop
{
    public sealed class CircleCurrencyRoulette : CircleRoulette
    {
        public override void DisplayRoulette<TChance>(ChanceCombiner<TChance> chances, int roulettesCount = 1, bool autoStart = true)
                        where TChance : class
        {
            if (typeof(TChance).Equals(typeof(CurrencyRouletteChance)))
            {
                base.DisplayRoulette(chances, roulettesCount, autoStart);
            }
            else
            {
                if (_isDebug) Debug.Log(name + ": TypeError!");
                Hide();
            }
        }

        protected override ConcreteReward GetConcreteReward<TChance>(ChanceCombiner<TChance> chances,
                                                                     int concreteChanceIndex = -1)
        {
            if (!typeof(TChance).Equals(typeof(CurrencyRouletteChance)))
            {
#if DEBUG
                if (_isDebug) Debug.Log(name + ": Chance type error!");
#endif
                return null;
            }

            if (concreteChanceIndex < 0)
            {
                TChance strikedObj = chances.GetStrikedObject();

                if (strikedObj == null) return null;

                int index = chances.SpawnChances.IndexOf(chances.SpawnChances.Find(item => item.Object.Equals(strikedObj)));

                return new CircleRouletteCurrencyReward
                (
                    strikedObj as CurrencyRouletteChance,
                    GetSlotData(chances.SpawnChances, index)
                );
            }

            return new CircleRouletteCurrencyReward
                (
                    chances.SpawnChances[concreteChanceIndex].Object as CurrencyRouletteChance,
                    GetSlotData(chances.SpawnChances, concreteChanceIndex)
                );
        }

        protected override Sprite GetSegmentSprite<TObject>(List<ObjectChanceSpawn<TObject>> chances, int chanceIndex)
            where TObject : class
        {
            return (chances[chanceIndex].Object as CurrencyRouletteChance).SegmentSprite;
        }

        protected override bool IsFillSegmentByAngle<TObject>(List<ObjectChanceSpawn<TObject>> chances, int chanceIndex)
        {
            return (chances[chanceIndex].Object as CurrencyRouletteChance).FillSegmentByAngle;
        }
    }
}