using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wordtris.GameCore.Functions.Dictionary.English
{
    public class UnityDictionaryLoader : IDictionaryLoader
    {
        public string LoadDictionaryContent(string rootLetter)
        {
            // Implement the actual loading logic, for example using Unity's Resources.Load
            var rawContent = Resources.Load<TextAsset>($"Dictionary/{rootLetter}");
            return rawContent?.text ?? string.Empty;
        }
    }


    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDict : DictionaryBase
    {
        private static readonly IReadOnlyDictionary<char, DictionaryBase> EngDictionaries;

        //public EngDict(IDictionaryLoader dictionaryLoader) : base(dictionaryLoader)
        //{
        //}

        static EngDict()
        {
            // Create an instance of your dictionary loader.
            var dictionaryLoader = new UnityDictionaryLoader(); // Example: Replace with your actual loader

            // Initialize the dictionary with each letter-specific dictionary.
            EngDictionaries = new Dictionary<char, DictionaryBase>
            {
                { 'A', new EngDictA(dictionaryLoader) },
                { 'B', new EngDictB(dictionaryLoader) },
                { 'C', new EngDictC(dictionaryLoader) },
                { 'D', new EngDictD(dictionaryLoader) },
                { 'E', new EngDictE(dictionaryLoader) },
                { 'F', new EngDictF(dictionaryLoader) },
                { 'G', new EngDictG(dictionaryLoader) },
                { 'H', new EngDictH(dictionaryLoader) },
                { 'I', new EngDictI(dictionaryLoader) },
                { 'J', new EngDictJ(dictionaryLoader) },
                { 'K', new EngDictK(dictionaryLoader) },
                { 'L', new EngDictL(dictionaryLoader) },
                { 'M', new EngDictM(dictionaryLoader) },
                { 'N', new EngDictN(dictionaryLoader) },
                { 'O', new EngDictO(dictionaryLoader) },
                { 'P', new EngDictP(dictionaryLoader) },
                { 'Q', new EngDictQ(dictionaryLoader) },
                { 'R', new EngDictR(dictionaryLoader) },
                { 'S', new EngDictS(dictionaryLoader) },
                { 'T', new EngDictT(dictionaryLoader) },
                { 'U', new EngDictU(dictionaryLoader) },
                { 'V', new EngDictV(dictionaryLoader) },
                { 'W', new EngDictW(dictionaryLoader) },
                { 'X', new EngDictX(dictionaryLoader) },
                { 'Y', new EngDictY(dictionaryLoader) },
                { 'Z', new EngDictZ(dictionaryLoader) },
            };
        }

        public EngDict(IDictionaryLoader dictionaryLoader) : base(dictionaryLoader)
        {
        }

        /// <inheritdoc />
        public override IReadOnlyList<WordDefinition> Define(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return Array.Empty<WordDefinition>();
            }

            var trimmedWord = word.Trim(); // remove empty chars

            var firstChar = char.ToUpperInvariant(trimmedWord[0]);

            if (char.IsLetter(firstChar)) // avoid special characters
            {
                return EngDictionaries[firstChar].Define(trimmedWord);
            }

            return Array.Empty<WordDefinition>();
        }

        protected override string RootLetter => string.Empty;

        /// <summary>
        /// Preload all the words to speed up lookup function.
        /// </summary>
        public void WarmUp()
        {
            foreach (var letter in EngDictionaries.Keys)
            {
                EngDictionaries[letter].Define($"{letter}");
            }
        }
    }
}
