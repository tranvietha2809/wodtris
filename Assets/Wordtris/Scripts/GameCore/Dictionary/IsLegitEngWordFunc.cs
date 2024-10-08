﻿
using Wordtris.GameCore.Functions.Lookup.English;

namespace Wordtris.GameCore.Functions
{
    /// <summary>
    /// Determines if a word is legit in English.
    /// </summary>
    public class IsLegitEngWordFunc : IsLegitWordFunc
    {
        public static readonly EngLookup EngLookup = new EngLookup();

        static IsLegitEngWordFunc()
        {
            EngLookup.WarmUp();
        }

        /// <inheritdoc />
        public override bool Invoke(string word)
        {
            var isLegitWord = EngLookup.Check(word);
            return isLegitWord;
        }
    }
}
