using System.Collections.Generic;

using UnityEngine;

namespace ZombieSurvival.Equipments
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Equipment/Equipment list", fileName = "New equipment list")]
    public class EquipmentList : ScriptableObject
    {
        [SerializeField] private List<EquipmentMaterialData> _materials;
        [SerializeField] private List<EquipmentData> _allEquipment;

        public EquipmentData this[EquipmentData data]
        {
            get
            {
                return _allEquipment.Find(item => item.Equals(data));
            }
        }

        public EquipmentData this[int id]
        {
            get
            {
                return _allEquipment.Find(item => item.ID == id);
            }
        }

        public EquipmentData GetRandomEquipment(EquipmentRarity rarity)
        {
            List<EquipmentData> random = _allEquipment.FindAll(item => item.EquipRarity.Equals(rarity));

            if (random.Count > 0) return random[Random.Range(0, random.Count)];
            else return null;
        }

        public EquipmentData GetRandomEquipment(EquipSlot slot)
        {
            List<EquipmentData> random = _allEquipment.FindAll(item => item.EquipSlot.Equals(slot));

            if (random.Count > 0) return random[Random.Range(0, random.Count)];
            else return null;
        }

        public EquipmentData GetRandomEquipment(EquipSlot slot, EquipmentRarity rarity)
        {
            List<EquipmentData> random = _allEquipment.FindAll(item => item.EquipSlot.Equals(slot) && 
                                                               item.EquipRarity.Equals(rarity));

            if (random.Count > 0) return random[Random.Range(0, random.Count)];
            else return null;
        }

        public EquipmentData GetRandomEquipment()
        {
            return _allEquipment[Random.Range(0, _allEquipment.Count)];
        }

        public EquipmentMaterialData GetRandomMaterial() => _materials[Random.Range(0, _materials.Count)];


        [ContextMenu("Set IDs")]
        public void SetIDs()
        {
            int id = 0;

            foreach (EquipmentData equipmentData in _allEquipment)
            {
                equipmentData.SetID(id);
                id++;
            }
        }
    }
}