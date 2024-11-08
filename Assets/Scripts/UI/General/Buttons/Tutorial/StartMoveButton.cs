using UnityEngine;
using UnityEngine.EventSystems;
using ZombieSurvival.General;

namespace ZombieSurvival.UI
{
    public sealed class StartMoveButton : ZSMonoBehaviour, IPointerDownHandler
    {
        private void Start()
        {
            Time.timeScale = 0;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Time.timeScale = 1;

            Destroy(gameObject);
        }
    }
}