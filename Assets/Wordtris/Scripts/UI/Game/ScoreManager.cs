using System;
using System.Collections;
using Wordtris.Scripts.Controller;
using Assets.Wordis.Frameworks.UITween.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Wordtris.Scripts.GamePlay
{
    /// <summary>
    /// Handled the game score.
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
#pragma warning disable 0649
        // Text displays score.
        [SerializeField] private Text txtScore;

        // Text displays best score for selected mode.
        [SerializeField] private Text txtBestScore;
#pragma warning restore 0649

        private int _score = 0;

        // Yield instruction for the score counter iterations.
        readonly WaitForSeconds _scoreIterationWait = new WaitForSeconds(0.02F);

#pragma warning disable 0649
        [SerializeField] private ScoreAnimator scoreAnimator;
#pragma warning restore 0649

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {

        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        void OnEnable()
        {
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
        }

        /// <summary>
        /// Set best score onn game start. 
        /// </summary>
        public void Init(string gameLevel)
        {
            txtScore.text = _score.ToString("N0");
            txtBestScore.text = GameProgressTracker.Instance.GetBestScore(gameLevel)
                .ToString("N0");
        }

        /// <summary>
        /// Adds score based on calculation and bonus.
        /// </summary>
        public void ShowScore(int score)
        {
            int oldScore = _score;

            StartCoroutine(SetScore(oldScore, score));
            scoreAnimator.Animate(score - oldScore);

            _score = score;
        }

        /// <summary>
        /// Returns score for the current game mode.
        /// </summary>
        public int GetScore()
        {
            return _score;
        }

        /// <summary>
        /// Set score with counter animation effect.
        /// </summary>
        private IEnumerator SetScore(int lastScore, int currentScore)
        {
            int iterationSize = (currentScore - lastScore) / 10;
            txtScore.transform.LocalScale(Vector3.one * 1.2F, 0.2F).OnComplete(() =>
            {
                txtScore.transform.LocalScale(Vector3.one, 0.2F);
            });

            for (int index = 1; index < 10; index++)
            {
                lastScore += iterationSize;
                txtScore.text = lastScore.ToString("N0");
                AudioController.Instance.PlayClipLow(
                    AudioController.Instance.addScoreSoundChord, 0.15F);
                yield return _scoreIterationWait;
            }

            txtScore.text = currentScore.ToString("N0");
        }

        /// <summary>
        /// Resets score on game over, game quit.
        /// </summary>
        public void Clear()
        {
            txtScore.text = "0";
            txtBestScore.text = "0";
            _score = 0;
        }
    }
}