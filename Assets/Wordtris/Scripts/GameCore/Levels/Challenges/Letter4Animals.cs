﻿using System;
using System.Linq;
using Wordtris.GameCore.Levels.Base;
using Wordtris.GameCore.Words;

namespace Wordtris.GameCore.Levels.Campaign
{
    /// <summary>
    /// 3-letter animals.
    /// See https://7esl.com/animals-vocabulary-animal-names/
    /// See https://bestforpuzzles.com/lists/animals/4.html
    /// </summary>
    public class Letter4Animals : WordisGameLevelBase<Letter4Animals>, IWordisGameLevel
    {
        public const int NeededMatches = 3;

        public static readonly WordisSettings LevelSettings = new WordisSettings(
            width: 9,
            height: 9,
            minWordLength: 3,
            waterLevel: 0, tetrominoBlock: true);

        /// <inheritdoc cref="Letter4Animals "/>
        public static WordsSequence Animals => WordsSequence.FromCsv(
@"Bear,Boar,Buck,Bull,Calf,Cavy,Colt,Cony,Coon,Deer,Foal,Gaur,Goat,Guib,
Hare,Ibex,Lamb,Lion,Lynx,Maki,Mara,Mare,Mink,Moke,Mole,Mona,Mule,Musk,
Orca,Oryx,Oxen,Puma,Seal,Vole,Wolf");

        private Letter4Animals(WordisGame game) : base(game)
        {
        }

        public Letter4Animals() : this(new WordisGame(
            LevelSettings,
            Animals
                .Shuffle()
                .AsLetterSource(shuffleWordLetters: true)))
        {
        }

        public override string Title => "4-letter animals"; // todo: localize

        public override string Goal =>
            $"Match {NeededMatches} animals\n" +
            $"Like '{Animals.Word.ToUpperInvariant()}'"; // todo: localize

        public override string Progress => $"{MatchedAnimals} of {NeededMatches} animals matched"; // todo: localize

        public override bool IsCompleted => MatchedAnimals >= NeededMatches;

        public override Letter4Animals WithUpdatedGame(WordisGame updatedGame) =>
            new Letter4Animals(updatedGame);

        private int MatchedAnimals => Game.Matches.All
                .Select(m => m.Word)
                .Intersect(Animals.Words, StringComparer.OrdinalIgnoreCase)
                .Count();
    }
}
