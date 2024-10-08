﻿using System;
using System.Linq;
using Wordtris.GameCore.Levels.Base;
using Wordtris.GameCore.Words;

namespace Wordtris.GameCore.Levels.Campaign
{
    /// <summary>
    /// 4-letters palindromes. 4-letters match.
    /// </summary>
    public class Letter4Palindromes : WordisGameLevelBase<Letter4Palindromes>, IWordisGameLevel
    {
        public const int NeededMatches = 10;

        public static readonly WordisSettings LevelSettings = new WordisSettings(
            width: 7,
            height: 7,
            minWordLength: 4,
            waterLevel: 3);

        /// <summary>
        /// https://en.wiktionary.org/wiki/Appendix:English_palindromes#Four_letters
        /// </summary>
        public static WordsSequence FourLetterPalindromes => WordsSequence.FromCsv(
            "peep,esse,anna,deed,poop,kook,noon");

        private Letter4Palindromes(WordisGame game) : base(game)
        {
        }

        /// <summary>
        /// Creates a level in its default state.
        /// </summary>
        public Letter4Palindromes() : this(
            new WordisGame(
                LevelSettings,
                FourLetterPalindromes
                    .Shuffle()
                    .AsLetterSource(shuffleWordLetters: true)))
        {
        }

        /// <inheritdoc cref="IWordisGameLevel" />
        public override string Title => "4-letter palindromes";

        /// <inheritdoc cref="IWordisGameLevel" />
        public override string Goal =>
            $"Match {NeededMatches} palindromes!\n" +
            $"Like '{FourLetterPalindromes.Word.ToUpperInvariant()}'";

        /// <inheritdoc cref="IWordisGameLevel.Progress" />
        public override string Progress => $"{MatchedPalindromes} of {NeededMatches} palindromes matched";

        /// <inheritdoc cref="IWordisGameLevel" />
        public override bool IsCompleted => MatchedPalindromes >= NeededMatches;

        /// <inheritdoc />
        public override Letter4Palindromes WithUpdatedGame(WordisGame updatedGame) =>
            new Letter4Palindromes(updatedGame);

        private int MatchedPalindromes =>
            Game.Matches.All
                .Select(m => m.Word)
                .Intersect(FourLetterPalindromes.Words, StringComparer.OrdinalIgnoreCase)
                .Count();
    }
}
