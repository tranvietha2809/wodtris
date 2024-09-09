using Wordtris.GameCore.Levels;
using Wordtris.Scripts.Controller;
using Wordtris.Scripts.UI.Extensions;
using Wordtris.Scripts.Utils.InputManager;
using UnityEngine;
using UnityEngine.UI;

namespace Wordtris.Scripts.Home
{
    public class HomeScreen : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] Button _btnPlay;
        [SerializeField] Button btnTimeMode;
        [SerializeField] Button btnAdvanceMode;
#pragma warning restore 0649

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        /// <summary>
        /// Classic mode button listener.
        /// </summary>
        public void OnClassicModeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIController.Instance.LoadGamePlay();
            }
        }

        /// <summary>
        /// Time mode button listener.
        /// </summary>
        public void OnTimeModeButtonPressed() // todo: drop
        {
        }

        /// <summary>
        /// Campaign button listener.
        /// </summary>
        public void OnTutorialModePressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIController.Instance.LoadGamePlay(new WordisTutorialLevel());
            }
        }

        /// <summary>
        /// Advance mode button listener.
        /// </summary>
        public void OnAdvanceModeButtonPressed() // todo: drop
        {
        }

        /// <summary>
        /// Advance mode button listener.
        /// </summary>
        public void OnCampaignModeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIController.Instance.selectLevelScreen.Activate();
            }
        }
    }
}