using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Equipments;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.UI.Shop
{
    public class EquipmentRoulette : LinearRoulette
    {
        [Header("Equipment roulette settings")]
        [SerializeField] private EquipmentTypesData _equipmentTypesData;
        [SerializeField] private EquipmentList _equipmentList;

        public override void DisplayRoulette<TChance>(ChanceCombiner<TChance> chances, int roulettesCount = 1, bool autoStart = true) 
                        where TChance : class
        {
            if (typeof(TChance).Equals(typeof(EquipmentRarity)))
            {
                base.DisplayRoulette(chances, roulettesCount, autoStart);
            }
            else
            {
                if (_isDebug) Debug.Log(name + ": TypeError!");
                Hide();
            }
        }

        public override void OnSkip()
        {
            base.OnSkip();

            EventBus.Publish<IOpenEquipmentChestHandler>(handler => handler.OnOpenEquipmentChest());
        }

        protected override ConcreteReward GetConcreteReward<TChance>(ChanceCombiner<TChance> chances,
                                                                     int concreteChanceIndex = -1)
        {
            if (typeof(TChance).Equals(typeof(EquipmentRarity)))
            {
                if (concreteChanceIndex == -1)
                {
                    return new NewEquipmentReward(_equipmentTypesData,
                        _equipmentList.GetRandomEquipment(chances.GetStrikedObject() as EquipmentRarity));
                }

                return new NewEquipmentReward(_equipmentTypesData,
                        _equipmentList.GetRandomEquipment(chances.SpawnChances[concreteChanceIndex].Object as EquipmentRarity));
            }
            else
            {
                if (_isDebug) Debug.Log(name + ": TypeError on getting concrete reward!");

                return null;
            }
        }

        protected override IEnumerator ShowRewardDelay(Reward reward)
        {
            yield return base.ShowRewardDelay(reward);

            EventBus.Publish<IOpenEquipmentChestHandler>(handler => handler.OnOpenEquipmentChest());
        }

        protected override IEnumerator ShowRewardDelay(ConcreteReward reward)
        {
            yield return base.ShowRewardDelay(reward);

            EventBus.Publish<IOpenEquipmentChestHandler>(handler => handler.OnOpenEquipmentChest());
        }

        protected override IEnumerator ShowRewardDelay(List<ConcreteReward> rewards)
        {
            yield return base.ShowRewardDelay(rewards);

            EventBus.Publish<IOpenEquipmentChestHandler>(handler => handler.OnOpenEquipmentChest());
        }
    }
}