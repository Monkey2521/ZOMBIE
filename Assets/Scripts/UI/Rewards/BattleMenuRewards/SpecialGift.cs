using UnityEngine;

using ZombieSurvival.General;

using Zenject;

namespace ZombieSurvival.Rewards.UI
{
    public class SpecialGift : MonoBehaviour
    {
        [SerializeField] private Reward _reward;

        [Inject] private MainInventory _mainInventory;

        public void GetGift()
        {
            _mainInventory.Add(new SpecialGiftReward(_reward));
        }
    }
}