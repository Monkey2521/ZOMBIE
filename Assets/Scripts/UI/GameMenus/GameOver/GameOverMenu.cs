using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using ZombieSurvival.Characters;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.Rewards;
using ZombieSurvival.Stats;
using ZombieSurvival.UI.Shop;

namespace ZombieSurvival.UI.GameMenus.GameOver
{
    public sealed class GameOverMenu : UIMenu, IPlayerDieHandler
    {
        [Header("Game over menu settings")]
        [SerializeField] private GOGameOverMenu _gameOverMenu;
        [SerializeField] private GOReanimationMenu _reanimationMenu;

        [Space(5)]
        [SerializeField] private ShopMenu _shop;
        [SerializeField] private ShopTab _currencyTab;

        private int _reanimations;
        private bool _onLoad;

        [Inject] private Player _player;
        [Inject] private MainInventory _mainInentory;

        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public override void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            base.Initialize(mainMenu, parentMenu);

            _gameOverMenu.Initialize(mainMenu, this);
            _reanimationMenu.Initialize(mainMenu, this);

            _reanimations = 0;

            _onLoad = false;
        }

        public override void Display(bool playAnimation = false)
        {
            base.Display(playAnimation);

            _reanimationMenu.Hide();
            _gameOverMenu.Hide();
        }

        public override void Hide(bool playAnimation = false)
        {
            base.Hide(playAnimation);

            _reanimationMenu.Hide(playAnimation);
            _gameOverMenu.Hide(playAnimation);
        }

        public void OnPlayerDie()
        {
            _mainMenu.Display(this);

            if (_reanimations < (_player.Stats as PlayerStats).RebornCount.Value)
            {
                _gameOverMenu.Hide();
                _reanimationMenu.Display(true);
            }
            else
            {
                _reanimationMenu.Hide();
                _gameOverMenu.Display(true);
            }
        }

        #region Buttons
        public void OnCurrencyReanimation()
        {
            if (_mainInentory.Spend(new CurrencyReward(_reanimationMenu.ReanimationCost)))
            {
                _player.GetUpgrade(_reanimationMenu.ReanimationByCurrencyHealUpgrade);
                _reanimations++;

                _mainMenu.DisplayDefault();

                _player.OnReanimation();
            }
            else
            {
                _shop.DisplayTab(_currencyTab, displayByMainMenu: false);

                _reanimationMenu.StopTimer();
            }
        }

        public void OnBackFromShop()
        {
            _reanimationMenu.ContinueTimer();
        }

        public void OnAdReanimation()
        {
            _player.GetUpgrade(_reanimationMenu.ReanimationByAdHealUpgrade);
            _reanimations++;

            _mainMenu.DisplayDefault();

            _player.OnReanimation();

            //_mainMenu.ShowPopupMessage("Ad is not ready");
        }

        public void OnCloseReanimation()
        {
            _reanimationMenu.Hide();
            _gameOverMenu.Display(true);
        }

        public void OnContinueGameOver()
        {
            if (_onLoad) return;

            _onLoad = true;

            var operation = SceneManager.LoadSceneAsync(GameData.MainMenuScene);

            operation.completed += new System.Action<AsyncOperation>((o) => Time.timeScale = 1);
        }
        #endregion
    }
}