using ZombieSurvival.Equipments;

namespace ZombieSurvival.Events
{
    public interface IGetEquipmentHandler : ISubscriber
    {
        public void OnGetEquipment(Equipment equipment);
    }
}