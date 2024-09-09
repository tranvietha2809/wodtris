using Wordtris.Scripts.Controller;
using Wordtris.Scripts.UI.Extensions;
using Wordtris.Scripts.Utils.InputManager;
using Assets.Wordis.Frameworks.ThemeManager.Scripts;
using UnityEngine;

namespace Wordtris.Scripts.UI
{
    /// <summary>
    /// Varies option button listener attached to this on home screen.
    /// </summary>
    public class OptionPanel : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] GameObject themeSettingButton;
#pragma warning restore 0649

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            if (!ThemeManager.Instance.UIThemeEnabled)
            {
                themeSettingButton.SetActive(false);
            }
        }

        /// <summary>
        /// Opens setting screen.
        /// </summary>
        public void OnSettingsButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay();
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.settingScreen.Activate();
            }
        }

        /// <summary>
        /// Opens shop popup.
        /// </summary>
        public void OnShopButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay();
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.shopScreen.Activate();
            }
        }

        /// <summary>
        /// Opens language selection popup.
        /// </summary>
        public void OnSelectLanguageButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay();
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.languageSelectionScreen.Activate();
            }
        }

        /// <summary>
        /// Opens statistics popup.
        /// </summary>
        public void OnStatsButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay();
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.statisticsScreen.Activate();
            }
        }

        /// <summary>
        /// Open theme selection popup.
        /// </summary>
        public void OnThemeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay();
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.selectThemeScreen.Activate();
            }
        }
    }
}