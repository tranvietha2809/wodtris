using Wordtris.GameCore.Objects;

namespace Wordtris.GameCore
{
    /// <summary>
    /// Represents possible in-game input made by the player.
    /// </summary>
    public class GameEvent
    {
        /// <summary>
        /// No input.
        /// </summary>
        public static readonly GameEvent None = new GameEvent();

        /// <summary>
        /// This is a primary event commanding the game to proceed to the next step.
        /// Evaluates success and failure conditions.
        /// </summary>
        public static readonly GameEvent Step = new GameEvent();

        /// <summary>
        /// Speed up the current <see cref="WordisObj"/> to be processed.
        /// </summary>
        public static readonly GameEvent Down = new GameEvent();

        /// <summary>
        /// Move the current <see cref="WordisObj"/> to the left.
        /// </summary>
        public static readonly GameEvent Left = new GameEvent();

        /// <summary>
        /// Move the current <see cref="WordisObj"/> to the right.
        /// </summary>
        public static readonly GameEvent Right = new GameEvent();

        public static readonly GameEvent RotateLeft = new GameEvent();

        public static readonly GameEvent RotateRight = new GameEvent();

        public static readonly GameEvent Missle = new GameEvent();

        public static readonly GameEvent Dynamite = new GameEvent();

        public static readonly GameEvent Hammer = new GameEvent();

        public static readonly GameEvent Bomb = new GameEvent();
        /// <summary>
        /// User tap on highlighted words to match.
        /// </summary>
        public static GameEvent Match(WordisChar @char)
        {
            return new MatchEvent(@char);
        }

        public static GameEvent Destroy(WordisChar @char)
        {
            return new MatchEvent(@char);
        }
    }

    public class MatchEvent : GameEvent
    {
        public MatchEvent(WordisChar @char)
        {
            this.Char = @char;
        }

        public WordisChar Char { get; private set; }
    }
}
