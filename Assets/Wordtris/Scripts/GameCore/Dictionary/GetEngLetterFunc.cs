using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Wordtris.GameCore.Functions
{
    /// <summary>
    /// Generates random English letters according to their frequency.
    /// </summary>
    public class GetEngLetterFunc : GetLetterFunc
    {
        private static readonly object RandomLock = new object();

        /// <summary>
        /// Random number generator.
        /// </summary>
        private static readonly Random Random = new Random(Environment.TickCount);

        /// <summary>
        /// English letter frequencies according to their occurrence in the language.
        /// </summary>
        /// <remarks>
        /// See <a href="https://en.wikipedia.org/wiki/Letter_frequency">Wikipedia: Letter Frequency</a> for more details.
        /// </remarks>
        private static readonly IReadOnlyDictionary<char, float> RelativeEngFrequency =
            new ReadOnlyDictionary<char, float>(new Dictionary<char, float>
            {
                { 'A', 0.0780f },
                { 'B', 0.0200f },
                { 'C', 0.0400f },
                { 'D', 0.0380f },
                { 'E', 0.1141f },
                { 'F', 0.0140f },
                { 'G', 0.0300f },
                { 'H', 0.0230f },
                { 'I', 0.0860f },
                { 'J', 0.0021f },
                { 'K', 0.0097f },
                { 'L', 0.0530f },
                { 'M', 0.0270f },
                { 'N', 0.0720f },
                { 'O', 0.0610f },
                { 'P', 0.0280f },
                { 'Q', 0.0019f },
                { 'R', 0.0730f },
                { 'S', 0.0870f },
                { 'T', 0.0670f },
                { 'U', 0.0330f },
                { 'V', 0.0100f },
                { 'W', 0.0091f },
                { 'X', 0.0027f },
                { 'Y', 0.0160f },
                { 'Z', 0.0044f },
            });

        /// <summary>
        /// Cumulative English letter frequencies for weighted random selection.
        /// </summary>
        /// <remarks>
        /// This is computed lazily to improve performance, as the cumulative distribution is only needed once.
        /// See the related discussion on <a href="https://stackoverflow.com/questions/2149914/randomly-generate-letters-according-to-their-frequency-of-use">StackOverflow</a>.
        /// </remarks>
        private static readonly Lazy<IReadOnlyDictionary<char, float>> CumulativeEngFrequency =
            new Lazy<IReadOnlyDictionary<char, float>>(() =>
                RelativeEngFrequency.ToDictionary(
                    p => p.Key,
                    p => p.Value +
                         RelativeEngFrequency
                             .Where(kvp => kvp.Key < p.Key)
                             .Sum(kvp => kvp.Value)));

        /// <inheritdoc />
        public override char Invoke()
        {
            lock (RandomLock)
            {
                // Generate a random number between 0 and 1.
                var randomPick = Random.NextDouble();

                // Find the first letter in the cumulative frequency distribution that exceeds the random pick.
                var randomChar = CumulativeEngFrequency.Value.First(p => p.Value > randomPick).Key;

                return randomChar;
            }
        }
    }
}
