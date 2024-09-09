using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wordtris.GameCore.ItemObjects;
using Assets.Wordis.Frameworks.Utils;
using Wordtris.Scripts.GamePlay;
using Wordtris.GameCore;

namespace Wordtris.Scripts.Controller
{
    public class ItemManager : Singleton<ItemManager>
    {
        private List<Item> playerItems;
        private const string ItemsPrefKey = "playerItems"; // Key for saving items in PlayerPrefs

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            playerItems = new List<Item>();
            LoadItems(); // Load saved items in Awake
        }

        /// <summary>
        /// Buys an item and deducts the corresponding currency.
        /// </summary>
        public void BuyItem(string itemName)
        {
            Item newItem = null;

            // Initialize the correct item based on the itemName
            if (itemName == "Bomb") newItem = new Bomb();
            else if (itemName == "Dynamite") newItem = new Dynamite();
            else if (itemName == "Hammer") newItem = new Hammer();
            else if (itemName == "Missile") newItem = new Missile();

            if (newItem != null && CurrencyManager.Instance.HasEnoughCurrency(newItem.Cost))
            {
                playerItems.Add(newItem); // Add item to player's list

                CurrencyManager.Instance.DeductGems(newItem.Cost); // Deduct currency
                SaveItems(); // Save updated inventory to PlayerPrefs

                //UIController.Instance.showPurchaseSuccessScreen();
            }
            else
            {
                Debug.Log($"Not enough points to buy {itemName} or invalid item name.");
            }
        }

        /// <summary>
        /// Uses an item if the player has it in inventory.
        /// </summary>
        public void UseItem(string name)
        {
            var item = playerItems.Find((item) => item.Name == name);
            if (item != null)
            {
                if (name == "Bomb") GamePlayUI.Instance.HandleGameEvent(GameEvent.Bomb);
                else if (name == "Dynamite") GamePlayUI.Instance.HandleGameEvent(GameEvent.Dynamite);
                else if (name == "Hammer") GamePlayUI.Instance.HandleGameEvent(GameEvent.Hammer);
                else if (name == "Missile") GamePlayUI.Instance.HandleGameEvent(GameEvent.Missle);

                item.UseItem();
                playerItems.Remove(item); // Remove item after use
                SaveItems(); // Save updated inventory to PlayerPrefs 

                Debug.Log($"{item.Name} used.");
            }
            else
            {
                Debug.Log($"You don't have {item.Name}.");
            }
        }

        /// <summary>
        /// Displays the player's current inventory.
        /// </summary>
        public void DisplayItems()
        {
            Debug.Log("Player items:");
            foreach (var item in playerItems)
            {
                Debug.Log($"{item.Name}");
            }
        }

        /// <summary>
        /// Saves the current inventory to PlayerPrefs.
        /// </summary>
        private void SaveItems()
        {
            var itemNames = playerItems.Select(item => item.GetType().Name).ToArray(); // Get item names
            string serializedItems = string.Join(",", itemNames); // Serialize the item list
            PlayerPrefs.SetString(ItemsPrefKey, serializedItems); // Save the serialized list to PlayerPrefs
        }

        /// <summary>
        /// Loads the saved inventory from PlayerPrefs.
        /// </summary>
        public void LoadItems()
        {
            if (PlayerPrefs.HasKey(ItemsPrefKey))
            {
                string serializedItems = PlayerPrefs.GetString(ItemsPrefKey);
                var itemNames = serializedItems.Split(','); // Deserialize the saved items

                playerItems = new List<Item>(); // Clear current items

                foreach (var itemName in itemNames)
                {
                    // Create item instances based on the saved names
                    if (itemName == "Bomb") playerItems.Add(new Bomb());
                    else if (itemName == "Dynamite") playerItems.Add(new Dynamite());
                    else if (itemName == "Hammer") playerItems.Add(new Hammer());
                    else if (itemName == "Missile") playerItems.Add(new Missile());
                }
            }
        }

        // Methods to get specific item counts
        public int GetDynamiteCount() => playerItems.Count(item => item is Dynamite);
        public int GetBombCount() => playerItems.Count(item => item is Bomb);
        public int GetHammerCount() => playerItems.Count(item => item is Hammer);
        public int GetMissileCount() => playerItems.Count(item => item is Missile);
    }
}
