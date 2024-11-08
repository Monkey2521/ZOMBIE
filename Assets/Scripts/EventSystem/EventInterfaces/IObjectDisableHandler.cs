using UnityEngine;

namespace ZombieSurvival.Events
{
    public interface IObjectDisableHandler : ISubscriber
    {
        public void OnObjectDisable(GameObject obj);
    }
}