﻿using Wordtris.Scripts.Controller;
using Wordtris.Scripts.UI.Extensions;
using Wordtris.Scripts.Utils.InputManager;
using UnityEngine;
using UnityEngine.UI;

namespace Wordtris.Scripts.UI.Utils
{
    /// <summary>
    /// This script is not in use.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class SettingButton : MonoBehaviour
    {
        Button btnSetting;

        private void Awake()
        {
            btnSetting = GetComponent<Button>();
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            btnSetting.onClick.AddListener(OnSettingsButtonClicked);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            btnSetting.onClick.RemoveListener(OnSettingsButtonClicked);
        }

        /// <summary>
        /// Setting button listener.
        /// </summary>
        void OnSettingsButtonClicked()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.settingScreen.Activate();
            }
        }
    }
}