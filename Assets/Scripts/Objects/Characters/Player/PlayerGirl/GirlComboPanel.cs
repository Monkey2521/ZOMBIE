using TMPro;

using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Interfaces;

namespace ZombieSurvival.Characters.Players
{
    public class GirlComboPanel : ZSMonoBehaviour, IPoolable
    {
        [Header("GirlComboPanel settings")]
        [SerializeField] private TMP_Text _comboText;

        private GirlComboCanvas _canvas;

        public void Initialize(GirlComboCanvas canvas, int combo)
        {
            _canvas = canvas;

            _comboText.text = "x" + combo + " COMBO!";
        }

        public void ResetObject()
        {
            _canvas = null;
        }

        public void Release()
        {
            _canvas.ReleasePanel(this);
        }
    }
}