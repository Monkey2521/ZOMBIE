using UnityEngine;
using UnityEngine.UI;

namespace ZombieSurvival.Achievements.UI
{
    public class AchievementLevel : MonoBehaviour
    {
        [SerializeField] private Image _levelImage;
        [SerializeField] private Sprite _lockedSprite;
        [SerializeField] private Sprite _unlockedSprite;

        public void Initialize(bool unlocked)
        {
            _levelImage.sprite = unlocked ? _unlockedSprite : _lockedSprite;
        }
    }
}