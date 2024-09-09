using System.Collections;
using System.Linq;
using Wordtris.GameCore.Levels.Base;
using Wordtris.Scripts.Controller;
using Wordtris.Scripts.GamePlay;
using Wordtris.Scripts.UI.Extensions;
using Wordtris.Scripts.Utils.InputManager;
using UnityEngine;
using UnityEngine.UI;

namespace Wordtris.Scripts.UI
{
    /// <summary>
    /// Code behind for Game Over and Level Completion
    /// </summary>
    public class GameOver : MonoBehaviour
    {
#pragma warning disable 0649

        [Tooltip("Game Over reason text")]
        [SerializeField]
        Text _txtGameOverTitle;

        [Tooltip("Score text from game over screen")]
        [SerializeField]
        Text txtScore;

        [Tooltip("BestScore text from game over screen")]
        [SerializeField]
        Text txtBestScore;

        [Tooltip("Reward Panel")]
        [SerializeField]
        GameObject rewardPanel;

        [Tooltip("Reward text from game over screen")]
        [SerializeField]
        Text txtReward;

        [SerializeField] GameObject gemImage;
        [SerializeField] GameObject rewardAnimation;
        [SerializeField] GameObject highScoreParticle;

        [SerializeField] GameObject nextGameBtn;
#pragma warning restore 0649

        private int _rewardAmount = 0;
        private int _gameOverId = 0;
        private IWordisGameLevel _gameLevel;

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            UIController.Instance.EnableCurrencyBalanceButton();
            TryShowingInterstitial();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            rewardAnimation.SetActive(false);
            UIController.Instance.DisableCurrencyBalanceButton();
        }

        /// <summary>
        /// Try to show Interstitial ad on game over if ad is available.
        /// </summary>
        private void TryShowingInterstitial()
        {
        }

        /// <summary>
        /// Sets game data and score on game over.
        /// </summary>
        public void SetGameData(
            int score,
            int totalWordsMatched,
            IWordisGameLevel gameLevel)
        {
            _gameLevel = gameLevel;

            if (gameLevel.IsCompleted)
            {
                _txtGameOverTitle.text = "Well Done!\nKeep up the great work.";
                _rewardAmount = (int)(score * 0.5);
                highScoreParticle.SetActive(true);
                nextGameBtn.SetActive(true);
            }
            else // level is failed
            {
                _txtGameOverTitle.text = "You've Got This!\nWant to try again?";
                _rewardAmount = (int)(score * 0.1);
                highScoreParticle.SetActive(false);
                nextGameBtn.SetActive(false);
            }

            txtScore.text = score.ToString("N0");
            txtReward.text = _rewardAmount.ToString("N0");

            GameProgressTracker.Instance.TrySetBestScore(score, gameLevel.Id);
            GameProgressTracker.Instance.SaveStats();
            GameProgressTracker.Instance.DropSession(gameLevel);

            txtBestScore.text = GameProgressTracker.Instance.GetBestScore(gameLevel.Id).ToString("N0");
            ShowRewardAnimation();
            // Number of time game over shown. Also total game play counts.
            _gameOverId = PlayerPrefs.GetInt("gameOverId", 0);
            _gameOverId += 1;
            PlayerPrefs.SetInt("gameOverId", _gameOverId);
        }

        private void ShowRewardAnimation()
        {
            CurrencyManager.Instance.AddGems(_rewardAmount);
            rewardAnimation.SetActive(true);
            gemImage.SetActive(true);
            rewardPanel.SetActive(true);
        }

        /// <summary>
        /// Continue button click listener.
        /// </summary>
        public void OnContinueButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                // select next level
                var nextLevels = SelectLevel.Levels
                    .SkipWhile(l => l.GetType() != _gameLevel.GetType())
                    .Skip(1) // this level
                    .ToArray();

                var nextLevel = nextLevels.Any()
                    ? nextLevels.First() // go next
                    : SelectLevel.Levels.First(); // start all over again

                // start next level
                StartCoroutine(RestartGame(nextLevel));
            }
        }

        /// <summary>
        /// Home button click listener.
        /// </summary>
        public void OnHomeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIController.Instance.OpenHomeScreenFromGameOver();
            }
        }

        /// <summary>
        /// Replay button click listener.
        /// </summary>
        public void OnReplayButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                StartCoroutine(RestartGame(_gameLevel));
            }
        }

        /// <summary>
        /// Restarts game.
        /// </summary>
        private IEnumerator RestartGame(IWordisGameLevel level)
        {
            yield return new WaitForSeconds(0.1f);
            GamePlayUI.Instance
                .SetLevel(level)
                .RestartGame();
            gameObject.Deactivate();
        }
    }
}