using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

using ZombieSurvival.Equipments;
using ZombieSurvival.General;
using ZombieSurvival.General.Inventories;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.UI.InventoryMenu
{
    public class MergeMenu : UIMenu
    {
        [Header("MergeMenu settings")]
        [SerializeField] private Image _hideButton;

        [Inject] private MainInventory _mainInventory;
        [Inject] private EquipmentInventory _equipmentInventory;
        [Inject] private EquipmentTypesData _equipmentTypesData;

        public override void Display(bool playAnimation = false)
        {
            _hideButton.raycastTarget = true;

            base.Display(playAnimation);
        }

        public override void Hide(bool playAnimation = false)
        {
            _hideButton.raycastTarget = false;

            base.Hide(playAnimation);
        }

        public void QuickMerge()
        {
            int index = 0;

            while (index < _equipmentInventory.Count)
            {
                if (_equipmentInventory[index].EquipmentData.NextRarityEquipment == null)
                {
                    index++;
                    continue;
                }

                List<Equipment> combines = _equipmentInventory.Equipment.FindAll(
                    item => item.EquipmentData.Equals(_equipmentInventory[index].EquipmentData));

                int requiredEquipmentAmount = _equipmentInventory[index].EquipmentData.NextRarityEquipment.EquipmentUpgrades.GetUpgrade(1).UpgradeMaterials.RequiredEquipmentAmount;

                if (combines != null && combines.Count >= requiredEquipmentAmount)
                {
                    _mainInventory.Add(new NewEquipmentReward
                        (
                            _equipmentTypesData,
                            combines[0].EquipmentData.MergeResult, 
                            combines.Count / requiredEquipmentAmount
                        ));

                    for (int i = 0; i < combines.Count / requiredEquipmentAmount * requiredEquipmentAmount; i++)
                    {
                        (_parentMenu as InventoryMenu).RemoveEquipment(combines[i]);

                        _mainInventory.Spend(new ConcreteEquipmentReward
                        (
                            _equipmentTypesData,
                            combines[i]
                        ));
                    }

                    index = 0;
                }
                else index++;
            }

            _parentMenu.Display();
        }
    }
}