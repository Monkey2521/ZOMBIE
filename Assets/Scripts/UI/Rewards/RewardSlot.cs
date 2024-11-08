using UnityEngine;
using UnityEngine.UI;

namespace ZombieSurvival.Rewards.UI
{
    public class RewardSlot : MonoBehaviour
    {
        [SerializeField] private Image _rewardIcon;
        [SerializeField] private Image _rewardBackground;
        [SerializeField] private Text _rewardAmountText;

        public void Initialize(ConcreteReward reward)
        {
            _rewardBackground.sprite = reward.Background;
            _rewardIcon.sprite = reward.Icon;
            
            _rewardAmountText.text = "x" + reward.Amount.ToString();

            if (reward.Amount == 1)
            {
                _rewardAmountText.enabled = false;
            }
            else
            {
                _rewardAmountText.enabled = true;
            }
        }
    }
}