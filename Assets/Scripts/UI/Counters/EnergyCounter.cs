using System;

using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.General.Inventories;

namespace ZombieSurvival.UI.Counters
{
    public class EnergyCounter : CurrencyCounter
    {
        [SerializeField] private Text _timeText;

        private int _timer;

        private void LateUpdate()
        {
            if (_inventory is EnergyInventory inventory)
            {
                DateTime now = DateTime.Now;

                if (inventory.Energy.Value < inventory.Energy.MaxValue && inventory.lastGainedTime == default)
                {
                    inventory.lastGainedTime = now;

                    _timer = inventory.EnergyRestoreCooldown - (int)(now - inventory.lastGainedTime).TotalSeconds;
                }
                else if (inventory.Energy.Value < inventory.Energy.MaxValue && inventory.lastGainedTime != default)
                {
                    _timer = inventory.EnergyRestoreCooldown - (int)(now - inventory.lastGainedTime).TotalSeconds;
                }
                else
                {
                    _timeText.enabled = false;
                    return;
                }

                if (_timer <= 0)
                {
                    inventory.Restore();
                }
            }
            else if (_isDebug) Debug.Log("Missing inventory!");

            _timeText.enabled = true;
            _timeText.text = IntegerFormatter.GetTime(_timer);
        }

        public override void UpdateCounter()
        {
            if (_inventory is EnergyInventory inventory)
            {
                _currencyText.text = ((int)inventory.Energy.Value).ToString() + "/" + ((int)inventory.Energy.MaxValue).ToString();
            }
            else if (_isDebug) Debug.Log("Missing inventory!");
        }
    }
}