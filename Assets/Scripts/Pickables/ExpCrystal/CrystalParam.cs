using UnityEngine;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public class CrystalParam
    {
        [Tooltip("Value that character get when take crystal")]
        [SerializeField] private int _expValue;
        [SerializeField] private Sprite _sprite;

        public int ExpValue => _expValue;
        public Sprite Sprite => _sprite;
    }
}