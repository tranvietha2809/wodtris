using Wordtris.GameCore.Levels.Base;
using Wordtris.GameCore.Words;

namespace Wordtris.GameCore.Levels.Campaign
{
    /// <summary>
    /// 3 letters match. Basic palindromes.
    /// </summary>
    public class Letter3Palindromes : WordisGameLevelBase<Letter3Palindromes>, IWordisGameLevel
    {
        public const int NeededMatches = 5;

        public static readonly WordisSettings LevelSettings = new WordisSettings(
            width: 5,
            height: 6,
            minWordLength: 3,
            waterLevel: 0);

        public static WordsSequence ThreeLetterPalindromes => WordsSequence.FromCsv(
            "lol,wow,eve,mom,dad,gig,pop,pup,ewe,did,gig,bob");

        private Letter3Palindromes(WordisGame game) : base(game)
        {
        }

        /// <summary>
        /// Creates a level in its default state.
        /// </summary>
        public Letter3Palindromes() : this(
            new WordisGame(
                LevelSettings,
                ThreeLetterPalindromes
                    .Shuffle()
                    .AsLetterSource()))
        {
        }

        /// <inheritdoc cref="IWordisGameLevel.Title" />
        public override string Title => "Basic palindromes"; // todo: localize

        /// <inheritdoc cref="IWordisGameLevel.Goal" />
        public override string Goal =>
            $"Match {NeededMatches} words!\n" +
            $"Like '{ThreeLetterPalindromes.Word.ToUpperInvariant()}'"; // todo: localize

        /// <inheritdoc cref="IWordisGameLevel.Progress" />
        public override string Progress => $"{Game.Matches.Count} of {NeededMatches} words matched";

        /// <inheritdoc cref="IWordisGameLevel" />
        public override bool IsCompleted =>
            Game.Matches.Count >= NeededMatches;

        public override Letter3Palindromes WithUpdatedGame(WordisGame updatedGame) =>
            new Letter3Palindromes(updatedGame);
    }
}
