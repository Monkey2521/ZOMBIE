using UnityEngine;

using ZombieSurvival.General;

using Zenject;

namespace ZombieSurvival.Rewards.UI
{
    public class TripForSupplies : MonoBehaviour
    {
        [Tooltip("Limit in seconds")]
        [SerializeField] private TripRewards _tripRewards;

        [Inject] private PlayerLevelInventory _playerLevelInventory;

        public TripForSuppliesRewardData GetCurrentRewards()
        {
            return _tripRewards.GetLevelRewards((int)_playerLevelInventory.PlayerLevel.Value);
        }
    }
}