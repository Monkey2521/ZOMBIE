using UnityEngine;
using UnityEngine.UI;

namespace ZombieSurvival.UI.Abilities
{
    public sealed class CombineAbilityUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Image _image;

        public void Initialize(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}