using System;
using System.Collections.Generic;
using System.Linq;
using Wordtris.GameCore;
using Wordtris.GameCore.Functions;
using Wordtris.GameCore.Levels;
using Wordtris.GameCore.Levels.Base;
using Wordtris.GameCore.Objects;
using Wordtris.Scripts.Controller;
using Wordtris.Scripts.UI;
using Wordtris.Scripts.UI.Extensions;
using Wordtris.Scripts.Utils.InputManager;
using Assets.Wordis.Frameworks.Utils;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Immutable;

namespace Wordtris.Scripts.GamePlay
{
    /// <summary>
    /// Encapsulates UI logic for the gameplay.
    /// Includes game events handling and UI refresh cycle.
    /// </summary>
    public class GamePlayUI : Singleton<GamePlayUI>
    {
        private static IWordisGameLevel DefaultLevel => new WordisSurvivalMode();

        private static readonly object GameLock = new object();

        private static readonly DefineEngWordFunc DefineWordFunc = new DefineEngWordFunc();

        private IWordisGameLevel _wordisGameLevel = DefaultLevel;

        private void GameStep() => HandleGameEvent(GameEvent.Step);

        /// <summary>
        /// Starts / resumes the game.
        /// </summary>
        public void ResumeGame()
        {
            PauseGame(); // to prevent double callback
            Invoke(nameof(GameStep), _wordisGameLevel.Settings.Speed);
        }

        /// <summary>
        /// Stops / pauses the game.
        /// </summary>
        public void PauseGame() => CancelInvoke(nameof(GameStep));

        public void HandleGameEvent(GameEvent gameEvent)
        {
            lock (GameLock)
            {
                PauseGame(); // prevent premature UI refresh

                if (_wordisGameLevel.IsCompleted || _wordisGameLevel.Game.IsGameOver)
                {
                    // stop the game cycle
                    PauseGame();
                    GameOver();
                    return;
                }

                var updatedLevel = _wordisGameLevel.Handle(gameEvent);

                if (updatedLevel.Game.GameEvents.Count >
                    _wordisGameLevel.Game.GameEvents.Count) // avoid extra refresh on game over.
                {
                    RefreshPresentation(updatedLevel.Game);
                    ShowProgress(updatedLevel.Progress);
                }

                _wordisGameLevel = updatedLevel;

                ResumeGame();
            }
        }

        /// <summary>
        /// Sets the level to be played.
        /// </summary>
        public GamePlayUI SetLevel(IWordisGameLevel gameLevel = null)
        {
            _wordisGameLevel = gameLevel ?? DefaultLevel;
            _wordisGameLevel = _wordisGameLevel.WithOutput(ShowMessage);

            return this;
        }

        /// <summary>
        /// Starts game with selected game mode.
        /// </summary>
        /// <param name="restore">Attempt to restore the last session.</param>
        public void RestartGame(bool restore = false)
        {
            if (restore)
            {
                //TryToRestoreGame();
            }
            else
            {
                ClearGame();
            }

            // Enables gameplay screen if not active.
            if (!gameBoard.gameObject.activeSelf)
            {
                gameBoard.gameObject.SetActive(true);
            }

            // Generated gameplay grid.
            gameBoard.boardGenerator.GenerateBoard(_wordisGameLevel.Settings);
            scoreManager.Init(_wordisGameLevel.Id);

            ShowMessage(_wordisGameLevel.Title); // move to TIP area? move to level?

            ShowMessage(_wordisGameLevel.Goal); // move to level?

            ResumeGame();
        }

        /// <summary>
        /// Pauses the game on pressing pause button.
        /// </summary>
        public void OnPauseButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIController.Instance.pauseGameScreen.Activate();
                GameProgressTracker.Instance.SaveSession(_wordisGameLevel);
                GameProgressTracker.Instance.SaveStats();
            }
        }

        [Tooltip("GamingButtonsController Script Reference")]
        public GamingButtonsController gamingButtonsController;

        [Tooltip("GamingSwipesController Script Reference")]
        public GamingSwipesController gamingSwipesController;

        [Header("Public Class Members")]
        [Tooltip("GamePlay Script Reference")]
        public GameBoard gameBoard;

        [Tooltip("ScoreManager Script Reference")]
        public ScoreManager scoreManager;

        [Tooltip("InGameMessage Script Reference To Show Message")]
        public InGameMessage inGameMessage;

        [Tooltip("Extra match animation")]
        public GameObject highScoreParticle;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Preload word definitions
            DefineWordFunc.WarmUp();
        }

        private void OnDisable() => ClearGame(dropSession: false);

        private void OnApplicationQuit() => GameProgressTracker.Instance.SaveSession(_wordisGameLevel);

        /// <summary>
        /// Will be called on game over.
        /// </summary>
        private void GameOver()
        {
            UIController.Instance.gameOverScreen
                .GetComponent<GameOver>()
                .SetGameData(
                    _wordisGameLevel.Game.Score.Value,
                    _wordisGameLevel.Game.Matches.Count,
                    _wordisGameLevel);

            UIController.Instance.gameOverScreen.Activate();
        }

        private void ClearGame(bool dropSession = true)
        {
            PauseGame();
            gameBoard.Clear();
            scoreManager.Clear();
            _wordisGameLevel = _wordisGameLevel.Reset();

            UIController.Instance.HideTips();

            if (dropSession)
            {
                GameProgressTracker.Instance.DropSession(_wordisGameLevel);
            }
        }

        private void TryToRestoreGame()
        {
            // todo: generalize
            if (_wordisGameLevel is WordisSurvivalMode survivalMode &&
                GameProgressTracker.Instance.HasSession(survivalMode))
            {
                var restoredGame = GameProgressTracker.Instance.RestoreSession(survivalMode);
                _wordisGameLevel = survivalMode.WithUpdatedGame(restoredGame ?? survivalMode.Game);
            }
            else
            {
                ClearGame();
            }
        }

        private void RefreshPresentation(WordisGame gameState)
        {
            // check last game event to avoid extra animations on user input
            if (gameState.LastEvent is MatchEvent &&
                gameState.Matches.Last.Any()) // on word match
            {
                // 1. display matches
                DisplayMatches(gameState.Matches.Last);
                // 2. refresh score
                scoreManager.ShowScore(gameState.Score.Value);
                // 3. update word matching stats
                GameProgressTracker.Instance.AddWordsStats(gameState.Matches.Last);
            }

            //DestroyBlocks(gameState.DestroyBlocks);

            var activeChars = gameState.ActiveChars; // can be null
            var availableMatches = new HashSet<WordisChar>(
                gameState.Matches.Available.SelectMany(m => m.MatchedChars));

            // 4. refresh every block on the board
            for (int x = 0; x < gameState.Matrix.Width; x++)
            {
                for (int y = 0; y < gameState.Matrix.Height; y++)
                {
                    var wordisObject = gameState.Matrix[x, y];

                    RefreshVisualBlock(x, y, wordisObject, activeChars, availableMatches);
                }
            }

            // 5. display a letter to come
            //if (activeChar?.Point != gameState.StartPoint)
            //{
            //    var backgroundColor = new Color(0.10f, 0.13f, 0.21f);
            //    var startBlock = gameBoard.allColumns[gameState.StartPoint.x][gameState.StartPoint.y];
            //    startBlock.SetText($"{gameState.LetterToCome}", backgroundColor);
            //}
        }

        private void DisplayMatches(IReadOnlyList<WordMatchEx> newMatches)
        {
            // 0. play nice animation on extra match
            if (newMatches.Count > 1)
            {
                highScoreParticle.GetComponent<ParticleSystem>().Play();
            }

            // 1. display matched words
            foreach (var match in newMatches)
            {
                ShowMessage(match.Word);
                ShowWordDefinition(match.Word);
            }

            var blocksToClear =
                newMatches
                    .SelectMany(match => match.MatchedChars)
                    .Select(c => gameBoard.allColumns[c.X][c.Y])
                    .ToArray();

            // 2. animate blocks destruction
            StartCoroutine(GameBoard.ClearAllBlocks(_wordisGameLevel.Settings, blocksToClear));

            // 3. play break sound
            AudioController.Instance.PlayLineBreakSound(blocksToClear.Length);
        }

        private void DestroyBlocks(ImmutableList<StaticChar> destroyBlocks)
        {
            var blocksToClear =
             destroyBlocks
                 .Select(c => gameBoard.allColumns[c.X][c.Y])
                 .ToArray();

            // 2. animate blocks destruction
            StartCoroutine(GameBoard.ClearAllBlocks(_wordisGameLevel.Settings, blocksToClear));

            // 3. play break sound
            AudioController.Instance.PlayLineBreakSound(blocksToClear.Length);
        }

        private void RefreshVisualBlock(
            int x,
            int y,
            WordisObj wordisObj,
            List<ActiveChar> activeChars,
            HashSet<WordisChar> matchedChars)
        {
            Block block = gameBoard.allColumns[x][y];
            block.RemoveAllListeners();

            if (wordisObj == null) // empty block
            {
                if (activeChars.Count > 0 &&
                    y > activeChars.Max(obj => obj.Y) &&
                    x >= activeChars.Min(obj => obj.X) &&
                    x <= activeChars.Max(obj => obj.X) &&
                    !_wordisGameLevel.Settings.IsWaterZone(y))
                {
                    // Highlight the trajectory
                    block.SetText(string.Empty, Color.white);
                    block.Highlight(Block.DefaultCharTag);
                }
                else
                {
                    // Either empty or 'water'
                    block.PlaceBlock(_wordisGameLevel.Settings.IsWaterZone(block.RowId)
                        ? Block.WaterTag
                        : block.defaultSpriteTag);
                    block.SetText(string.Empty, Color.white);
                }
            }
            else // letter block
            {
                var isMatchedChar = matchedChars.Contains(wordisObj);

                string spriteTag = string.Empty;
                if (isMatchedChar)
                {
                    spriteTag = Block.MatchedCharTag;
                }
                else if (wordisObj is WordisChar)
                {
                    if ((wordisObj as WordisChar).isDynamite())
                    {
                        spriteTag = Block.DynamiteTag;
                    }
                    else
                    if ((wordisObj as WordisChar).isBomb())
                    {
                        spriteTag = Block.DynamiteTag;
                    }
                    else
                    {
                        spriteTag = Block.DefaultCharTag;
                    }
                }
                block.PlaceBlock(spriteTag);

                if (isMatchedChar)
                {
                    block.ShakeAnimation();
                }

                if (wordisObj is WordisChar wordisChar && !wordisChar.isDynamite() && !wordisChar.isBomb())
                {
                    block.AddOnClickListener(EmmitWordMatch(wordisObj as WordisChar));

                    block.SetText($"{wordisChar.Value}", Color.white);
                    Debug.Log(wordisChar);
                }
            }
        }

        System.Action EmmitWordMatch(WordisChar wordisChar)
        {
            return () => HandleGameEvent(GameEvent.Match(wordisChar));
        }

        private void ShowWordDefinition(string word)
        {
            var definitions = DefineWordFunc.Invoke(word);

            if (definitions.Any())
            {
                UIController.Instance.ShowTopTipAtPosition(
                    tipPosition: new Vector2(0, -250F), // todo: make default, dont specify in code
                    anchor: new Vector2(0.5F, 1), // todo: make default, dont specify in code
                    tipText: $"{definitions[0].Word}: {definitions[0].Definition}",
                    duration: 7F);
            }
        }

        /// <summary>
        /// Try to display level progression.
        /// </summary>
        private void ShowProgress(string levelProgress)
        {
            if (!string.IsNullOrWhiteSpace(levelProgress))
            {
                UIController.Instance.ShowDownTipAtPosition(
                    tipPosition: new Vector2(0, 400F), // todo: make default, dont specify in code
                    anchor: new Vector2(0.5F, 0), // todo: make default, dont specify in code
                    tipText: levelProgress,
                    duration: 7F);
            }
        }

        private void ShowMessage(string message) =>
            inGameMessage.ShowMessage(message);

    }
}