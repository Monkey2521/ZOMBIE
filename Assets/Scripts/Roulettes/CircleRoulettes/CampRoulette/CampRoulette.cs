using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.UI.Shop
{
    public sealed class CampRoulette : CircleRoulette
    {
        public override void DisplayRoulette<TChance>(ChanceCombiner<TChance> chances, int roulettesCount = 1, bool autoStart = true)
                        where TChance : class
        {
            if (typeof(TChance).Equals(typeof(CampRoulleteChance)))
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
            if (!typeof(TChance).Equals(typeof(CampRoulleteChance)))
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

                return new CampUpgradeReward
                (
                    strikedObj as CampRoulleteChance,
                    GetSlotData(chances.SpawnChances, index)
                );
            }

            return new CampUpgradeReward
                (
                    chances.SpawnChances[concreteChanceIndex].Object as CampRoulleteChance,
                    GetSlotData(chances.SpawnChances, concreteChanceIndex)
                );
        }

        protected override Sprite GetSegmentSprite<TObject>(List<ObjectChanceSpawn<TObject>> chances, int chanceIndex)
            where TObject : class
        {
            return (chances[chanceIndex].Object as CampRoulleteChance).SegmentSprite;
        }

        protected override bool IsFillSegmentByAngle<TObject>(List<ObjectChanceSpawn<TObject>> chances, int chanceIndex)
        {
            return (chances[chanceIndex].Object as CampRoulleteChance).FillSegmentByAngle;
        }
    }
}