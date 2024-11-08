using UnityEngine;
using ZombieSurvival.General;
using ZombieSurvival.Objects;

namespace ZombieSurvival.Abilities
{
    public sealed class ObjectCatcher : TriggerDetector
    {
        [SerializeField] private Transform _carrier;

        protected override void OnTriggerEnter(Collider other)
        {
            if (_triggerTags.Contains(other.tag))
            {
                PickableObject obj = other.GetComponent<PickableObject>();

                if (obj != null && !obj.IsPickUpped)
                {
                    obj.PickUp(_carrier);
                }
                else if (_isDebug) Debug.Log("Missing pickable object!");
            }
        }
    }
}