﻿using System.Collections;
using System.Linq;
using Wordtris.GameCore.Functions;
using Wordtris.GameCore.Levels;
using Wordtris.Scripts.Controller;
using Wordtris.Scripts.GamePlay;
using Wordtris.Scripts.UI.Extensions;
using Wordtris.Scripts.Utils.InputManager;
using UnityEngine;
using UnityEngine.UI;

namespace Wordtris.Scripts.UI
{
    /// <summary>
    /// Selection on mode to be played.
    /// </summary>
    public class Statistics : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] GameObject _statItemTemplate;
        [SerializeField] GameObject _rootContent;
        [SerializeField] GameObject _statsListContent;
        [SerializeField] GameObject _wordsUnlockedCounter;
        [SerializeField] GameObject _maxScoreCounter;
#pragma warning restore 0649

        /// <summary>
        /// Close button listener.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                gameObject.Deactivate();
            }
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void OnEnable()
        {
            PrepareStatsScreen();
            // scroll to the top
            StartCoroutine(nameof(ScrollToTop));
        }

        private IEnumerator ScrollToTop()
        {
            var scrollRect = _rootContent.GetComponent<ScrollRect>();
            yield return new WaitForEndOfFrame();
            scrollRect.gameObject.SetActive(true);
            scrollRect.verticalNormalizedPosition = 1f;
        }

        private void PrepareStatsScreen()
        {
            var progressTracker = GameProgressTracker.Instance;
            var wordStats = progressTracker.GetWordStats();

            // display highest score (only survival mode for now)
            SetMaxScoreCounter(progressTracker.GetBestScore(nameof(WordisSurvivalMode)));

            // display unlocked words counter
            SetWordsUnlockedCounter(wordStats.Count);

            // remove dynamic items, i = 2 stands for existing 2 buttons
            for (int i = 2; i < _statsListContent.transform.childCount; i++)
            {
                _statsListContent.transform.GetChild(i).gameObject.Deactivate();
            }

            // display unlocked words ordered by alphabet
            foreach (var wordStat in wordStats.OrderBy(p => p.Key))
            {
                CreateWordStatItem(wordStat.Key, wordStat.Value);
            }
        }

        private void SetWordsUnlockedCounter(int uniqueWordsUnlocked)
        {
            _wordsUnlockedCounter.GetComponent<Text>().text = $"{uniqueWordsUnlocked}";
        }

        private void SetMaxScoreCounter(int maxScore)
        {
            _maxScoreCounter.GetComponent<Text>().text = $"{maxScore}";
        }

        /// <summary>
        /// Instantiates a button from template.
        /// </summary>
        /// <returns></returns>
        private void CreateWordStatItem(string word, int counter)
        {
            GameObject statItem = Instantiate(_statItemTemplate);
            statItem.transform.SetParent(_statsListContent.transform);
            statItem.name = $"itm-{word}";
            statItem.transform.localScale = Vector3.one;
            statItem.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            statItem.transform.SetAsLastSibling();
            statItem.GetComponentsInChildren<Text>().First().text = word;
            statItem.GetComponentsInChildren<Text>().Last().text = $"{counter}";
            statItem.SetActive(true);

            // set word definition callback
            statItem.GetComponent<Button>().onClick.AddListener(() => ShowDefinition(word));
        }

        /// <summary>
        /// Level button listener
        /// </summary>
        private void ShowDefinition(string word)
        {
            Debug.LogWarning("ShowDefinition  " + word);

            var defineFn = new DefineEngWordFunc();
            var definitions = defineFn.Invoke(word);

            if (definitions.Any())
            {
                UIController.Instance.ShowMessage(word, definitions[0].FullDefinition);
            }
        }
    }
}