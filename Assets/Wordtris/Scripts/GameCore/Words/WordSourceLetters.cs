using System.Linq;
using Wordtris.GameCore.Extensions;

namespace Wordtris.GameCore.Words
{
    /// <summary>
    /// Represents a sequence of letters derived from a sequence of words.
    /// </summary>
    public class WordSourceLetters : LetterSource
    {
        // Private fields to store the word source, letter source, and whether to shuffle letters in a word.
        private readonly WordSource _wordSource;
        private readonly LetterSource _letterSource;
        private readonly bool _shuffleWordLetters;

        /// <summary>
        /// Private constructor to initialize a new instance of the WordSourceLetters class.
        /// </summary>
        /// <param name="wordSource">The source of words.</param>
        /// <param name="letterSource">The source of letters within a word.</param>
        /// <param name="shuffleWordLetters">Indicates whether to shuffle the letters within a word.</param>
        private WordSourceLetters(
            WordSource wordSource,
            LetterSource letterSource,
            bool shuffleWordLetters)
        {
            _wordSource = wordSource;
            _letterSource = letterSource;
            _shuffleWordLetters = shuffleWordLetters;
        }

        /// <summary>
        /// Public constructor to initialize a new instance of the WordSourceLetters class.
        /// This constructor is used to create a letter source from a word source.
        /// </summary>
        /// <param name="wordSource">The source of words.</param>
        /// <param name="shuffleWordLetters">Indicates whether to shuffle the letters within a word.</param>
        public WordSourceLetters(
            WordSource wordSource,
            bool shuffleWordLetters = false)
            : this(
                wordSource,
                new WordLetters(
                    shuffleWordLetters
                    ? new string(wordSource.Word.ToCharArray().Shuffle().ToArray()) // Shuffle letters if required.
                    : wordSource.Word), // Use the original word if no shuffling is needed.
                shuffleWordLetters)
        {
        }

        /// <summary>
        /// Gets the current character from the letter source.
        /// </summary>
        /// <returns>The current character.</returns>
        protected override char GetChar() => _letterSource.Char;

        /// <summary>
        /// Gets the next letter source in the sequence.
        /// </summary>
        public override LetterSource Next =>
            IsLast
                ? new WordSourceLetters(_wordSource.Next, _shuffleWordLetters) // Move to the next word if the current one is exhausted.
                : _letterSource.IsLast
                    ? new WordSourceLetters(_wordSource.Next, _shuffleWordLetters) // Move to the next word if the current letter is the last one.
                    : new WordSourceLetters(_wordSource, _letterSource.Next, _shuffleWordLetters); // Move to the next letter in the current word.

        /// <summary>
        /// Checks if the current letter source is the last in the sequence.
        /// </summary>
        public override bool IsLast => _wordSource.IsLast && _letterSource.IsLast; // True if both the word and letter sources are exhausted.
    }
}
