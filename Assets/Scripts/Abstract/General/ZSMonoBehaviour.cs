using UnityEngine;

namespace ZombieSurvival.General
{
    public abstract class ZSMonoBehaviour : MonoBehaviour
    {
        [Header("Debug settings")]
        [SerializeField] protected bool _isDebug;

        protected void Log(string message)
        {
#if DEBUG
            if (_isDebug) Debug.Log(name + " : " + message);
#endif
        }
    }
}