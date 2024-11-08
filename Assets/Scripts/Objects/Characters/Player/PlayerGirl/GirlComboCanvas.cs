using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Interfaces;
using ZombieSurvival.Objects;

namespace ZombieSurvival.Characters.Players
{
    public class GirlComboCanvas : ZSMonoBehaviour, IFixedUpdatable
    {
        [Header("GirlCombo Canvas settings")]
        [SerializeField] private GirlComboPanel _panelPrefab;
        [SerializeField] private Transform _panelsParent;

        private MonoPool<GirlComboPanel> _pool;

        public void Initialize()
        {
            _pool = new MonoPool<GirlComboPanel>(_panelPrefab, 2, _panelsParent);
        }

        public void DisplayCombo(int combo)
        {
            GirlComboPanel panel = _pool.Pull();

            panel.Initialize(this, combo);
        }

        public void ReleasePanel(GirlComboPanel panel)
        {
            _pool.Release(panel);
        }

        public void OnFixedUpdate()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}