using UnityEngine;

using Zenject;

using ZombieSurvival.Equipments;

[CreateAssetMenu(fileName = "EquipmentTypesDataInstaller", menuName = "Installers/EquipmentTypesDataInstaller")]
public class EquipmentTypesDataInstaller : ScriptableObjectInstaller<EquipmentTypesDataInstaller>
{
    [SerializeField] private EquipmentTypesData _equipmentTypesData;

    public override void InstallBindings()
    {
        Container.Bind<EquipmentTypesData>().FromScriptableObject(_equipmentTypesData).AsSingle();
    }
}
