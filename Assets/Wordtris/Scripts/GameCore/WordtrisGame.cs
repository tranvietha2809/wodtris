using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Wordtris.GameCore.Functions;
using Wordtris.GameCore.Objects;
using Wordtris.GameCore.Words;
using Newtonsoft.Json;
using UnityEngine;

namespace Wordtris.GameCore
{
    public class WordisGame
    {
        // TetrominoChar representing the active piece (current tetromino on the board)
        private readonly TetrominoChar _activePiece;

        // TetrominoChar representing the next piece (next tetromino to spawn)
        private readonly TetrominoChar _nextPiece;

        // RandomTetrominoWithLettersSource to generate tetrominos with random letters
        private readonly RandomTetrominoWithLettersSource _randomTetrominoWithLettersSource;

        // Function to find word matches in the matrix
        private readonly FindWordMatchesFunc _findWordMatchesFunc;

        // Immutable list of available word matches in the game
        [JsonProperty("availableMatches")]
        private readonly ImmutableList<WordMatchEx> _availableMatches;

        // Immutable list of word matches that occurred during the game
        [JsonProperty("wordMatches")]
        private readonly ImmutableList<WordMatchEx> _wordMatches;

        // Immutable list of the last matches in the game
        [JsonProperty("lastMatches")]
        private readonly ImmutableList<WordMatchEx> _lastMatches;

        // Immutable list of game events such as moves or actions taken during the game
        [JsonProperty("gameEvents")]
        private readonly ImmutableList<GameEvent> _gameEvents;

        // Immutable list of game objects (e.g., tetrominos, letters) currently in the game
        private readonly ImmutableList<WordisObj> _gameObjects;

        // Immutable list of static characters (blocks that are no longer active)
        private readonly ImmutableList<StaticChar> _destroyObjects;

        // Constructor for creating a new game instance with all properties provided
        [JsonConstructor]
        private WordisGame(
            WordisSettings settings,
            LetterSource letterSource,
            FindWordMatchesFunc findWordMatchesFunc,
            RandomTetrominoWithLettersSource randomTetrominoWithLetters,
            ImmutableList<WordisObj> gameObjects,
            ImmutableList<WordMatchEx> wordMatches,
            ImmutableList<WordMatchEx> lastMatches,
            ImmutableList<WordMatchEx> availableMatches,
            ImmutableList<GameEvent> gameEvents,
            ImmutableList<StaticChar> destroyObjects,
            TetrominoChar nextPiece)
        {
            Settings = settings;
            Matrix = new WordisMatrix(this); // Initialize the game matrix
            _findWordMatchesFunc = findWordMatchesFunc ?? new FindWordMatchesFunc(); // Default function for word matching
            _randomTetrominoWithLettersSource = randomTetrominoWithLetters
                                                ?? new RandomTetrominoWithLettersSource(null, letterSource, settings.TetrominoBlock); // Default random tetromino source
            _gameObjects = gameObjects ?? ImmutableList<WordisObj>.Empty; // Initialize game objects
            _wordMatches = wordMatches ?? ImmutableList<WordMatchEx>.Empty; // Initialize word matches
            _lastMatches = lastMatches ?? ImmutableList<WordMatchEx>.Empty; // Initialize last matches
            _availableMatches = availableMatches ?? ImmutableList<WordMatchEx>.Empty; // Initialize available matches
            _gameEvents = gameEvents ?? ImmutableList<GameEvent>.Empty; // Initialize game events
            _destroyObjects = destroyObjects ?? ImmutableList<StaticChar>.Empty; // Initialize destroyed objects

            // Find active game piece (if any) and initialize _activePiece and _nextPiece
            var activeObj = _gameObjects.FirstOrDefault(o => o is ActiveChar); // Look for any active tetromino
            if (activeObj != null && Enum.TryParse(activeObj.GroupId, out BlockType tetrominoType))
            {
                _activePiece = new TetrominoChar(tetrominoType, new List<ActiveChar>(GameObjects.OfType<ActiveChar>().ToList()));

                if (nextPiece == null)
                {
                    var nextTetromino = _randomTetrominoWithLettersSource.Next(null);
                    _nextPiece = new TetrominoChar(nextTetromino.GetTetrominoType(),
                        nextTetromino.GenerateTetrominoChar(StartPoint.x, StartPoint.y));
                }
                else
                {
                    _nextPiece = nextPiece;
                }
            }
        }

        // Public constructor for initializing a game instance with fewer parameters
        public WordisGame(
            WordisSettings settings,
            LetterSource letterSource = null,
            FindWordMatchesFunc findWordMatchesFunc = null,
            RandomTetrominoWithLettersSource randomTetrominoWithLetters = null,
            TetrominoChar nextPiece = null) : this(
                settings: settings,
                letterSource: letterSource,
                findWordMatchesFunc: findWordMatchesFunc,
                gameObjects: null,
                wordMatches: null,
                lastMatches: null,
                availableMatches: null,
                gameEvents: null,
                randomTetrominoWithLetters: randomTetrominoWithLetters,
                destroyObjects: null, nextPiece: nextPiece)
        {
        }

        // Game settings containing dimensions, block size, etc.
        public WordisSettings Settings { get; }

        // Returns the list of game objects (tetrominos, letters)
        public IReadOnlyList<WordisObj> GameObjects => _gameObjects;

        // Gets the list of active characters on the board (from the active tetromino)
        [JsonIgnore]
        public List<ActiveChar> ActiveChars => _activePiece == null ? new List<ActiveChar>() : _activePiece.Blocks;

        // Game matrix that represents the grid of letters
        [JsonIgnore]
        public WordisMatrix Matrix { get; }

        // Returns the list of all game events
        public IReadOnlyList<GameEvent> GameEvents => _gameEvents;

        // Returns the most recent game event
        [JsonIgnore]
        public GameEvent LastEvent => GameEvents.Any() ? GameEvents[^1] : GameEvent.None;

        // Returns the word matches found in the game
        [JsonIgnore]
        public WordMatches Matches => new WordMatches(this);

        // Returns the game score based on words matched
        [JsonIgnore]
        public WordisScore Score => new WordisScore(this);

        // Returns the list of destroyed blocks (StaticChar objects)
        [JsonIgnore]
        public ImmutableList<StaticChar> DestroyBlocks => _destroyObjects;

        // Checks whether the game is over
        public bool IsGameOver
        {
            get
            {
                // Check if the next piece will overlap with any existing blocks
                if (_nextPiece != null)
                {
                    foreach (var block in _nextPiece.Blocks)
                    {
                        int checkX = block.X;
                        int checkY = block.Y;

                        if (Matrix[checkX, checkY] != null && !(Matrix[checkX, checkY] is ActiveChar))
                        {
                            Debug.Log($"Game over detected at position ({checkX}, {checkY})");
                            return true;
                        }
                    }
                }

                // Check if the top of the board is filled
                for (int y = 0; y < Settings.Height; y++)
                {
                    Debug.Log(Matrix[StartPoint.x, y] + "gameover check");
                    if (Matrix[StartPoint.x, y] == null || Matrix[StartPoint.x, y] is ActiveChar)
                    {
                        return false; // No game over if there is space at the top
                    }
                }

                return true; // Game over if no space at the top
            }
        }

        // Handles game events such as movements, matches, and steps
        public virtual WordisGame Handle(GameEvent gameEvent)
        {
            if (IsGameOver) return this; // Stop if the game is over

            var updatedEvents = _gameEvents.Add(gameEvent); // Add new game event

            // Split game objects into static and active objects
            var staticObjects = _gameObjects.Where(o => !(o is ActiveChar)).ToList();
            var activeObjects = _gameObjects.OfType<ActiveChar>().ToList();
            var remainingObjects = _gameObjects.RemoveAll(o => o is ActiveChar); // Remove active objects

            // Handle static objects and update the game state accordingly

            var staticHandled = staticObjects.ToImmutableList();

            if (!Settings.TetrominoBlock)
            {
                staticHandled = staticObjects.Select(o => o.Handle(this, gameEvent, activeObjects)).ToImmutableList();
            }

            ImmutableList<StaticChar> destroyHandled = ImmutableList<StaticChar>.Empty;

            // Handle destruction of static objects (e.g., falling blocks)
            staticObjects.ForEach(o =>
            {
                var list = (o as StaticChar)?.BombDestroyHandle(this).ToImmutableList();
                if (list != null) destroyHandled = destroyHandled.AddRange(list);
            });

            // Filter out destroyed objects
            var filteredStaticHandled = staticHandled.Except(destroyHandled).ToImmutableList();

            // Create updated game state with static objects handled
            var updatedGame = With(
                gameObjects: filteredStaticHandled,
                gameEvents: updatedEvents,
                lastMatches: ImmutableList<WordMatchEx>.Empty,
                destroyObjects: destroyHandled);

            // Handle active piece movement or events
            if (_activePiece != null)
            {
                List<WordisObj> activeHandled = new List<WordisObj>();

                if (!Settings.TetrominoBlock)
                {
                    var newList = activeObjects.Select(o => o.Handle(this, gameEvent, activeObjects)).ToList();
                    activeHandled = newList;
                }
                else
                {
                    activeHandled = _activePiece.Handle(this, gameEvent);
                }
                activeHandled.AddRange(staticHandled); // Combine active and static object handling

                updatedGame = With(
                    gameObjects: activeHandled.ToImmutableList(),
                    gameEvents: updatedEvents,
                    lastMatches: ImmutableList<WordMatchEx>.Empty);
            }

            if (gameEvent == GameEvent.Dynamite)
            {
                var bombPiece = new TetrominoChar(BlockType.Bomb,
                    _randomTetrominoWithLettersSource.Next(BlockType.Bomb).GenerateTetrominoChar(StartPoint.x, StartPoint.y));
                return updatedGame.CanHaveActiveChars ?
                    updatedGame.WithNewActiveChars(bombPiece) :
                    updatedGame.With(nextPiece: bombPiece);
            }

            // Handle step event (advance game logic)
            if (gameEvent == GameEvent.Step)
            {
                var availableMatches = FindWordMatches(updatedGame.Matrix);
                updatedGame = updatedGame.With(availableMatches: availableMatches);
                if (updatedGame.CanHaveActiveChars)
                {
                    return updatedGame.WithNewActiveChars(_nextPiece);
                }
                else
                {
                    return updatedGame;
                }
            }

            // Handle match event (when a word is matched)
            if (gameEvent is MatchEvent matchEvent)
            {
                var match = updatedGame._availableMatches
                    .Where(a => a.CharsSet.Contains(matchEvent.Char))
                    .OrderByDescending(a => a.MatchedChars.Length)
                    .Take(1)
                    .ToImmutableList();

                updatedGame = match.Any()
                    ? updatedGame.With(
                        gameObjects: updatedGame._gameObjects
                            .Except(match.SelectMany(m => m.MatchedChars))
                            .ToImmutableList(),
                        wordMatches: updatedGame._wordMatches.AddRange(match),
                        lastMatches: match,
                        availableMatches: ImmutableList<WordMatchEx>.Empty)
                    : updatedGame;
                return updatedGame.CanHaveActiveChars ? updatedGame.WithNewActiveChars(_nextPiece) : updatedGame;
            }

            return updatedGame; // Return the updated game state
        }

        // Utility method to create a new game state with updated properties
        private WordisGame With(
            LetterSource letterSource = null,
            RandomTetrominoWithLettersSource randomTetrominoWithLetters = null,
            ImmutableList<WordisObj> gameObjects = null,
            ImmutableList<WordMatchEx> wordMatches = null,
            ImmutableList<WordMatchEx> lastMatches = null,
            ImmutableList<WordMatchEx> availableMatches = null,
            ImmutableList<GameEvent> gameEvents = null,
            ImmutableList<StaticChar> destroyObjects = null,
            TetrominoChar nextPiece = null) =>
            new WordisGame(
                settings: Settings,
                letterSource: letterSource,
                randomTetrominoWithLetters: randomTetrominoWithLetters ?? _randomTetrominoWithLettersSource,
                findWordMatchesFunc: _findWordMatchesFunc,
                gameObjects: gameObjects ?? _gameObjects,
                gameEvents: gameEvents ?? _gameEvents,
                wordMatches: wordMatches ?? _wordMatches,
                lastMatches: lastMatches ?? _lastMatches,
                availableMatches: availableMatches ?? _availableMatches,
                destroyObjects: destroyObjects ?? _destroyObjects,
                nextPiece: nextPiece ?? _nextPiece);

        public WordisGame With(List<ActiveChar> gameObj) =>
            With(gameObjects: _gameObjects.AddRange(gameObj));

        public WordisGame WithWordMatches(params WordMatchEx[] wordMatches) =>
            With(wordMatches: wordMatches.ToImmutableList());

        // StartPoint defines the starting position of new tetrominos
        public (int x, int y) StartPoint => (x: Settings.Width / 2, y: 0);

        // Find word matches in the matrix
        private ImmutableList<WordMatchEx> FindWordMatches(WordisMatrix matrix)
        {
            return _findWordMatchesFunc
                .Invoke(matrix, Settings.MinWordLength)
                .Select(m => new WordMatchEx(m, _gameEvents.Count, DateTimeOffset.UtcNow))
                .ToImmutableList();
        }

        // Checks if new active chars can be generated
        private bool CanHaveActiveChars =>
            !IsGameOver && _activePiece == null && Matrix[StartPoint.x, StartPoint.y] == null;

        // Generates a new active tetromino piece

        private WordisGame WithNewActiveChars(TetrominoChar nextPiece = null)
        {
            // Move the next piece to the active piece
            List<ActiveChar> newActiveChars = new List<ActiveChar>();
            if (nextPiece == null)
            {
                newActiveChars = _randomTetrominoWithLettersSource.GenerateTetrominoChar(StartPoint.x, StartPoint.y);
            }
            else
            {
                newActiveChars = nextPiece.Blocks;
            }


            // Return updated game state with the new active piece and next piece
            return With(
                randomTetrominoWithLetters: _randomTetrominoWithLettersSource.Next(null),
                gameObjects: _gameObjects.AddRange(newActiveChars) // Set the new active piece
            );
        }

        private WordisGame WithSpecificActiveChars(BlockType blockType) =>
            With(_randomTetrominoWithLettersSource.GenerateTetrominoChar(StartPoint.x, StartPoint.y))
                .With(randomTetrominoWithLetters: _randomTetrominoWithLettersSource.Next(null));

        // WordMatches class to provide access to various match-related lists
        public class WordMatches
        {
            private readonly WordisGame _game;

            public WordMatches(WordisGame game)
            {
                _game = game;
            }

            public IReadOnlyList<WordMatchEx> All => _game._wordMatches;
            public IReadOnlyList<WordMatchEx> Last => _game._lastMatches ?? ImmutableList<WordMatchEx>.Empty;
            public IReadOnlyList<WordMatchEx> Available => _game._availableMatches;
            public int Count => _game._wordMatches.Count;
        }

        // Serialization method to convert game state to JSON
        public static string ToJson(WordisGame game)
        {
            return JsonConvert.SerializeObject(game, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        // Deserialization method to create a game instance from JSON
        public static WordisGame FromJson(string jsonStr)
        {
            return JsonConvert.DeserializeObject<WordisGame>(jsonStr, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}
