using System;
using System.Collections.Generic;

namespace Wordtris.GameCore.Functions.Lookup.English
{
    /// <summary>
    /// English words lookup.
    /// </summary>
    /// <summary>
    /// English words lookup using Aho-Corasick algorithm.
    /// </summary>
    public class EngLookup : WordLookupBase
    {
        private static readonly IReadOnlyDictionary<char, WordLookupBase> EngLookups =
            new Dictionary<char, WordLookupBase>
            {
                { 'A', new EngLookupA() },
                { 'B', new EngLookupB() },
                { 'C', new EngLookupC() },
                { 'D', new EngLookupD() },
                { 'E', new EngLookupE() },
                { 'F', new EngLookupF() },
                { 'G', new EngLookupG() },
                { 'H', new EngLookupH() },
                { 'I', new EngLookupI() },
                { 'J', new EngLookupJ() },
                { 'K', new EngLookupK() },
                { 'L', new EngLookupL() },
                { 'M', new EngLookupM() },
                { 'N', new EngLookupN() },
                { 'O', new EngLookupO() },
                { 'P', new EngLookupP() },
                { 'Q', new EngLookupQ() },
                { 'R', new EngLookupR() },
                { 'S', new EngLookupS() },
                { 'T', new EngLookupT() },
                { 'U', new EngLookupU() },
                { 'V', new EngLookupV() },
                { 'W', new EngLookupW() },
                { 'X', new EngLookupX() },
                { 'Y', new EngLookupY() },
                { 'Z', new EngLookupZ() },
            };

        private static readonly AhoCorasickTrie Trie = new AhoCorasickTrie();

        static EngLookup()
        {
            // Populate the Aho-Corasick Trie with all words from each lookup
            foreach (var lookup in EngLookups.Values)
            {
                var words = lookup.GetWords();
                foreach (var word in words)
                {
                    Trie.AddWord(word);
                }
            }
            Trie.Build();
        }

        /// <inheritdoc />
        public override bool Check(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return false;
            }

            var trimmedWord = word.Trim(); // remove empty chars

            var firstChar = char.ToUpperInvariant(trimmedWord[0]);

            if (char.IsLetter(firstChar)) // avoid special characters
            {
                // Use the original method for single word lookup
                return EngLookups[firstChar].Check(trimmedWord);
            }

            return false;
        }

        /// <inheritdoc />
        protected override string WordsInCsv => string.Empty;

        /// <summary>
        /// Preload all the words to speed up the lookup function.
        /// </summary>
        public void WarmUp()
        {
            foreach (var letter in EngLookups.Keys)
            {
                EngLookups[letter].Check($"{letter}");
            }
        }

        /// <summary>
        /// Check for multiple words using the Aho-Corasick algorithm.
        /// </summary>
        public List<string> CheckMultiple(string text)
        {
            return Trie.Search(text);
        }
    }
}
