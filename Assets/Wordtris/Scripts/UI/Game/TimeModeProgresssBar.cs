﻿using Wordtris.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Wordtris.Scripts.GamePlay
{
    /// <summary>
    /// This script is typically used for time mode only to control the game timer.
    /// </summary>
    public class TimeModeProgresssBar : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] public Image imgTimerBar;
#pragma warning restore 0649


        [System.NonSerialized] public bool promptedTimeOver = false;

        private float _maxTimer = 0;
        private float _remainingTime = 0;

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
        }

        /// <summary>
        /// Game-over callback.
        /// </summary>
        private void GamePlayUI_OnGameOverEvent()
        {
            StopTimer();
        }

        /// <summary>
        /// Game pause callback.
        /// </summary>
        private void GamePlayUI_OnGamePausedEvent(bool paused)
        {
            if (paused)
            {
                PauseTimer();
            }
            else
            {
                ResumeTimer();
            }
        }

        /// <summary>
        /// Starts the timer and will keep invoking each second in repeatative mode.
        /// </summary>
        public void StartTimer()
        {
            if (!IsInvoking(nameof(StartContinuousTimer)))
            {
                InvokeRepeating(nameof(StartContinuousTimer), 1, 1);
            }
        }

        /// <summary>
        /// Paused the timer. Will act similar to stop timer.
        /// </summary>
        public void PauseTimer()
        {
            if (IsInvoking(nameof(StartContinuousTimer)))
            {
                CancelInvoke(nameof(StartContinuousTimer));
            }
        }

        /// <summary>
        /// Stops the timer. 
        /// </summary>
        public void StopTimer()
        {
            if (IsInvoking(nameof(StartContinuousTimer)))
            {
                CancelInvoke(nameof(StartContinuousTimer));
            }
        }

        /// <summary>
        /// Resumes the timer from current state.
        /// </summary>
        public void ResumeTimer()
        {
            StartTimer();
        }

        /// <summary>
        /// Will be called on starting of game. Amount of time will be fetched from game setting and if game has previos progress then the time amount will be given from thr previous sesison progress data.
        /// </summary>
        public void SetTimer(float seconds)
        {
            // _maxTimer = GamePlayUI.Instance.timeModeInitialTimer;

            _remainingTime = seconds;
            _remainingTime = Mathf.Clamp(_remainingTime, 0, _maxTimer);
            imgTimerBar.fillAmount = GetFillAmount();
            promptedTimeOver = false;
        }

        /// <summary>
        /// Adds given seconds to current running timer. Value will be clamped down to max possible time amount if exceeding after adding time.
        /// </summary>
        public void AddTime(float seconds)
        {
            _remainingTime += seconds;
            _remainingTime = Mathf.Clamp(_remainingTime, 0, _maxTimer);
        }

        /// <summary>
        /// Returns remaining time amount.
        /// </summary>
        public int GetRemainingTimer()
        {
            return (int)_remainingTime;
        }

        /// <summary>
        /// This method will be executed each second while timer is running.
        /// </summary>
        private void StartContinuousTimer()
        {
            if (_remainingTime > 0)
            {
                _remainingTime -= 1;
            }

            //imgTimerBar.FillAmount(GetFillAmount(), 1F).SetEase(Ease.Linear);

            if (_remainingTime <= 0)
            {
                StopTimer();
                if (!(
                      UIController.Instance.gameOverScreen.activeSelf))
                {
                    // GamePlayUI.Instance.TryRescueGame(GameOverReason.TimeOver);
                }
            }
        }

        /// <summary>
        /// Returns the Image fill amount of progress bar based on remaining timer out of full timer.
        /// </summary>
        private float GetFillAmount()
        {
            return _remainingTime / _maxTimer;
        }
    }
}