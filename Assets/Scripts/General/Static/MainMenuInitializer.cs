using System.Collections.Generic;
using UnityEngine;
using ZombieSurvival.UI;

namespace ZombieSurvival.General
{
    public class MainMenuInitializer : ZSMonoBehaviour
    {
        [SerializeField] private List<MainInventory> _inventories;
        [SerializeField] private MainMenu _mainMenu;

        protected virtual void Start()
        {
            foreach(MainInventory inventory in _inventories)
            {
                inventory.Initialize();
            }

            _mainMenu.Initialize();
        }
    }
}