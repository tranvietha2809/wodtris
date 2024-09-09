using Wordtris.Scripts.Controller;
using Wordtris.Scripts.GamePlay;
using Wordtris.Scripts.UI.Extensions;
using Wordtris.Scripts.Utils.InputManager;
using UnityEngine;

namespace Wordtris.Scripts.UI
{
    /// <summary>
    /// This script component is attached to game pause screen.
    /// </summary>
    public class PauseGame : MonoBehaviour
    {
        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            // Pauses the game when it gets enabled.
            GamePlayUI.Instance.PauseGame();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            // Resumes the game when it gets enabled.
            GamePlayUI.Instance.ResumeGame();
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        public void OnResumeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                gameObject.Deactivate();
            }
        }

        /// <summary>
        /// Restarts the game.
        /// </summary>
        public void OnRestartButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                GamePlayUI.Instance.RestartGame();
                gameObject.Deactivate();
            }
        }

        /// <summary>
        /// Closes the gameplay and navigates to home screen.
        /// </summary>
        public void OnHomeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.OpenHomeScreenFromPauseGame();
            }
        }

        /// <summary>
        /// Closes pause screen and resumes gameplay.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                gameObject.Deactivate();
            }
        }
    }
}