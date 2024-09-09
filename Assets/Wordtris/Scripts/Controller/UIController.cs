using System.Collections;
using System.Collections.Generic;
using Wordtris.GameCore.Levels.Base;
using Wordtris.Scripts.GamePlay;
using Wordtris.Scripts.Home;
using Wordtris.Scripts.UI.Extensions;
using Wordtris.Scripts.Utils.InputManager;
using Assets.Wordis.Frameworks.UITween.Scripts.Utils;
using Assets.Wordis.Frameworks.Utils;
using UnityEngine;
using Wordtris.Scripts.UI;

namespace Wordtris.Scripts.Controller
{
    /// <summary>
    /// UIController controls the entire UI Navigation of the game.
    /// </summary>
    public class UIController : Singleton<UIController>
    {
        readonly List<string> _screenStack = new List<string>();

        [SerializeField] Canvas UICanvas;

        [Header("UI Screens")]
        public HomeScreen homeScreen;
        public GamePlayUI gameScreen;

        [Header("Public Members.")]
        public GameObject shopScreen;
        public GameObject settingScreen;
        public GameObject selectLevelScreen;
        public GameObject pauseGameScreen;
        public GameObject selectThemeScreen;
        public GameObject purchaseSuccessScreen;
        public GameObject commonMessageScreen;
        public GameObject dailyRewardScreen;
        public GameObject gameOverScreen;
        public GameObject languageSelectionScreen;
        public GameObject statisticsScreen;
        public GameObject currencyBalanceButton;
        public GameObject itemButtonPanel;

        public GameObject topTipView;
        public GameObject downTipView;

        [Header("Other Public Members")]
        public RectTransform ShopButtonGemsIcon;
        public Transform RuntimeEffectSpawnParent;

        // Ordered popup stack is used when another popup tries to open when already a popup is opened. Ordered stack will control it and add upcoming popups
        // to queue so it will load automatically when already existing popup gets closed.
        readonly List<string> _orderedPopupStack = new List<string>();

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            // Enables home screen on game start.

            homeScreen.gameObject.Activate();
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            // Registers session update callback.
            SessionManager.OnSessionUpdatedEvent += OnSessionUpdated;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            // Un-registers session update callback.
            SessionManager.OnSessionUpdatedEvent -= OnSessionUpdated;
        }

        /// <summary>
        /// Session Updated callback.
        /// </summary>
        private void OnSessionUpdated(SessionInfo info)
        {
        }

        /// <summary>
        /// Handles the device back button, this will be used for android only. 
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (InputManager.Instance.canInput())
                {
                    if (_screenStack.Count > 0)
                    {
                        ProcessBackButton(Peek());
                    }
                }
            }
        }

        /// <summary>
        /// Adds the latest activated GameObject to stack.
        /// </summary>
        public void Push(string screenName)
        {
            if (!_screenStack.Contains(screenName))
            {
                _screenStack.Add(screenName);
            }
        }

        /// <summary>
        /// Returns the name of last activated GameObject from the stack.
        /// </summary>
        public string Peek()
        {
            if (_screenStack.Count > 0)
            {
                return _screenStack[_screenStack.Count - 1];
            }

            return string.Empty;
        }

        /// <summary>
        /// Removes the last GameObject name from the stack.
        /// </summary>
        public void Pop(string screenName)
        {
            if (_screenStack.Contains(screenName))
            {
                _screenStack.Remove(screenName);

                if (_orderedPopupStack.Contains(screenName))
                {
                    _orderedPopupStack.Remove(screenName);

                    if (_orderedPopupStack.Count > 0)
                    {
                        ShowDialogFromStack();
                    }
                }
            }
        }

        /// <summary>
        /// On pressing back button of device, the last added popup/screen will get deactivated based on state of the game. 
        /// </summary>
        private void ProcessBackButton(string currentScreen)
        {
            switch (currentScreen)
            {
                case nameof(HomeScreen):
                    QuitGamePopup();
                    break;
                case nameof(SelectLevel):
                    selectLevelScreen.Deactivate();
                    break;
                case nameof(Statistics):
                    statisticsScreen.Deactivate();
                    break;
                case "GamePlay":
                    break;
                case "Shop":
                    shopScreen.Deactivate();
                    break;
                case "Settings":
                    settingScreen.Deactivate();
                    break;
                case "CommonMessageScreen":
                    commonMessageScreen.Deactivate();
                    break;
                case "PurchaseSuccessScreen":
                    purchaseSuccessScreen.Deactivate();
                    break;
                case "SelectLanguage":
                    languageSelectionScreen.Deactivate();
                    break;
                case "PauseGame":
                    pauseGameScreen.Deactivate();
                    break;
            }
        }

        /// <summary>
        /// Opens a quit game popup.
        /// </summary>
        private void QuitGamePopup()
        {
            new CommonDialogueInfo().SetTitle("Quit")
                //.SetMessage(LocalizationManager.Instance.GetTextWithTag("txtQuitConfirm"))
                //.SetPositiveButtomText(LocalizationManager.Instance.GetTextWithTag("txtNo"))
                //.SetNegativeButtonText(LocalizationManager.Instance.GetTextWithTag("txtYes"))
                .SetMessageType(CommonDialogueMessageType.Confirmation)
                .SetOnPositiveButtonClickListener(() => { Instance.commonMessageScreen.Deactivate(); })
                .SetOnNegativeButtonClickListener(() =>
                {
                    QuitGame();
                    Instance.commonMessageScreen.Deactivate();
                }).Show();
        }

        // Quits the game.
        public void QuitGame()
        {
            Invoke(nameof(QuitGameAfterDelay), 0.5F);
        }

        /// <summary>
        /// Quits game after little delay.  Waiting for poup animation to get completed.
        /// </summary>
        void QuitGameAfterDelay()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
                //On Android, on quitting app, App actually won't quit but will be sent to background. So it can be load fast while reopening. 
                AndroidJavaObject activity =
 new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call<bool>("moveTaskToBack" , true);
#elif UNITY_IOS
                Application.Quit();
#endif
        }

        /// <summary>
        /// Show common pop-up. 
        /// </summary>
        public void ShowMessage(string title, string message)
        {
            new CommonDialogueInfo().SetTitle(title).SetMessage(message).SetMessageType(CommonDialogueMessageType.Info)
                .SetOnConfirmButtonClickListener(() => { Instance.commonMessageScreen.Deactivate(); })
                .Show();
        }

        /// <summary>
        /// Opens Daily Reward screen if day has changed.
        /// </summary>
        public void ShowDailyRewardsPopup()
        {
            _orderedPopupStack.Add(dailyRewardScreen.name);
            ShowDialogFromStack();
        }


        /// <summary>
        /// Controls the ordered stack.
        /// </summary>
        private void ShowDialogFromStack()
        {
            if (_orderedPopupStack.Count > 0)
            {
                string screenName = _orderedPopupStack[0];

                switch (screenName)
                {
                    case "DailyRewardScreen":
                        if (!dailyRewardScreen.activeSelf)
                        {
                            dailyRewardScreen.Activate();
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Disables home and select mode screen and opens gameplay.
        /// </summary>
        public void LoadGamePlay(IWordisGameLevel gameLevel = null)
        {
            homeScreen.gameObject.Deactivate();

            gameScreen.gameObject.Activate();
            gameScreen
                .GetComponent<GamePlayUI>()
                .SetLevel(gameLevel)
                .RestartGame(restore: true);
        }

        /// <summary>
        /// Open Home screen when user presses home button from gameover screen.
        /// </summary>
        public void OpenHomeScreenFromGameOver()
        {
            StartCoroutine(OpenHomeScreenFromGameOverCoroutine());
        }

        private IEnumerator OpenHomeScreenFromGameOverCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            gameScreen.gameObject.Deactivate();
            gameOverScreen.Deactivate();
            homeScreen.gameObject.Activate();
        }

        /// <summary>
        /// Open Home screen when user presses home button from pause screen during gameplay.
        /// </summary>
        public void OpenHomeScreenFromPauseGame()
        {
            StartCoroutine(OpenHomeScreenFromPauseGameCoroutine());
        }

        private IEnumerator OpenHomeScreenFromPauseGameCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            gameScreen.gameObject.Deactivate();
            pauseGameScreen.Deactivate();
            homeScreen.gameObject.Activate();
        }

        /// <summary>
        /// Enables currency balance button. Currency balance button will be shown during shop screen, reward adding or reducing current only.
        /// </summary>
        public void EnableCurrencyBalanceButton()
        {
            currencyBalanceButton.GetComponent<CanvasGroup>().SetAlpha(1, 1);
        }

        /// <summary>
        /// Disable currency balance button.
        /// </summary>
        public void DisableCurrencyBalanceButton()
        {
            if (!(
                  gameOverScreen.activeSelf ||
                  shopScreen.activeSelf ||
                  purchaseSuccessScreen.activeSelf))
            {
                if (currencyBalanceButton != null &&
                    currencyBalanceButton.activeSelf)
                {
                    currencyBalanceButton.GetComponent<CanvasGroup>().SetAlpha(0, 0.3F);
                }
            }
        }

        public void PlayAddGemsAnimationAtPosition(Vector3 position, float delay)
        {
            GameObject rewardAnim = (GameObject)Instantiate(Resources.Load("RewardAnimation"));
            rewardAnim.transform.SetParent(RuntimeEffectSpawnParent);
            rewardAnim.GetComponent<RectTransform>().position = position;
            rewardAnim.transform.localScale = Vector3.one;

        }

        public void PlayDeductGemsAnimation(Vector3 position, float delay)
        {
            GameObject rewardAnim = (GameObject)Instantiate(Resources.Load("RewardAnimation"));
            rewardAnim.transform.SetParent(RuntimeEffectSpawnParent);
            rewardAnim.GetComponent<RectTransform>().position = ShopButtonGemsIcon.position;
            rewardAnim.transform.localScale = Vector3.one;
        }

        public void ShowTopTipAtPosition(
            Vector2 tipPosition,
            Vector2 anchor,
            string tipText,
            float duration)
        {
            topTipView.GetComponent<TipView>().ShowTipAtPosition(
                tipPosition: tipPosition,
                anchor: anchor,
                tipText: tipText,
                duration: duration);

            topTipView.Activate(false);
        }

        public void ShowDownTipAtPosition(
            Vector2 tipPosition,
            Vector2 anchor,
            string tipText,
            float duration)
        {
            downTipView.GetComponent<TipView>().ShowTipAtPosition(
                tipPosition: tipPosition,
                anchor: anchor,
                tipText: tipText,
                duration: duration);

            downTipView.Activate(false);
        }

        public void HideTips()
        {
            topTipView.GetComponent<TipView>().HideTip();
            downTipView.GetComponent<TipView>().HideTip();
        }

        public void EnableItemPanel()
        {
            itemButtonPanel.SetActive(true);
        }

        public void HideItemPanel()
        {
            itemButtonPanel?.SetActive(false);
        }

        public void showPurchaseSuccessScreen()
        {
            purchaseSuccessScreen.SetActive(true);
        }
    }
}