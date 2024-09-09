using Wordtris.Scripts.Controller;
using Wordtris.Scripts.UI.Extensions;
using Wordtris.Scripts.Utils.InputManager;
using UnityEngine;
using UnityEngine.UI;

namespace Wordtris.Scripts.UI
{
    public class ShopScreen : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] RectTransform mainContentRect;

#pragma warning restore 0649

        Vector2 currentContentSize;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            currentContentSize = mainContentRect.sizeDelta;
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            UIController.Instance.EnableCurrencyBalanceButton();
            UpdateShopScreen();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            // Don't hide gems button if rescue screen is open.
            UIController.Instance.Invoke("DisableCurrencyBalanceButton", 0.1F);
        }

        /// <summary>
        /// Close button click listener.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                gameObject.Deactivate();
            }
        }

        /// <summary>
        /// Purchase button click listener.
        /// </summary>
        public void OnPurhcaseBombButtonClicked()
        {
            if (InputManager.Instance.canInput())
            {
                ItemManager.Instance.BuyItem("Bomb");
            }
        }

        public void OnPurhcaseDynamiteButtonClicked()
        {
            if (InputManager.Instance.canInput())
            {
                ItemManager.Instance.BuyItem("Dynamite");
            }
        }

        /// <summary>
        /// Purchase button click listener.
        /// </summary>
        public void OnGetFreeGemsButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
            }
        }

        void UpdateShopScreen()
        {

        }
    }
}