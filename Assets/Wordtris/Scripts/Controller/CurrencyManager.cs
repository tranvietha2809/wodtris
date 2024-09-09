using System;
using Wordtris.Scripts.UI.Extensions;
using Assets.Wordis.Frameworks.Utils;
using UnityEngine;

namespace Wordtris.Scripts.Controller
{
    /// <summary>
    /// This script controls and manages the in-game currency, its balance, addition or subtraction of balance.
    /// </summary>
    public class CurrencyManager : Singleton<CurrencyManager>
    {
        public static event Action<int> OnCurrencyUpdated;

        int currentBalance;
        bool hasInitialised = false;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            Initialise();
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
        }

        /// <summary>
        /// This function is called when the object becomes disabled and inactive.
        /// </summary>
        private void OnDisable()
        {
        }

        /// <summary>
        /// Initialize and restores the balance  and displays it.
        /// </summary>
        void Initialise()
        {
            if (PlayerPrefs.HasKey("currentBalance"))
            {
                currentBalance = PlayerPrefs.GetInt("currentBalance");
            }
            else
            {
            }

            hasInitialised = true;
        }

        /// <summary>
        /// Returns current balance.
        /// </summary>
        public int GetCurrentGemsBalance()
        {
            if (!hasInitialised)
            {
                Initialise();
            }

            return currentBalance;
        }


        /// <summary>
        /// Add gems to current balance.
        /// </summary>
        public void AddGems(int gemsAmount)
        {
            if (gemsAmount > 0)
            {
                currentBalance += gemsAmount;
            }

            SaveCurrencyBalance();
            OnCurrencyUpdated?.Invoke(currentBalance);

            AudioController.Instance.PlayClip(AudioController.Instance.addGemsSound);
        }

        /// <summary>
        /// Will deduct given amount from balance if enough balance is available.
        /// </summary>
        public bool DeductGems(int gemsAmount)
        {
            if (currentBalance >= gemsAmount)
            {
                currentBalance -= gemsAmount;
                SaveCurrencyBalance();

                OnCurrencyUpdated?.Invoke(currentBalance);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Save currency balance.
        /// </summary>
        void SaveCurrencyBalance()
        {
            PlayerPrefs.SetInt("currentBalance", currentBalance);
        }

        public bool HasEnoughCurrency(int cost)
        {
            return currentBalance >= cost;
        }
    }
}