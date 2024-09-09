using System.Collections.Generic;
using Wordtris.GameCore.Functions.Dictionary;
using Wordtris.GameCore.Functions.Dictionary.English;

namespace Wordtris.GameCore.Functions
{
    /// <summary>
    /// Gets english word definitions
    /// </summary>
    public class DefineEngWordFunc : DefineWordFunc
    {
        public static readonly UnityDictionaryLoader dictionaryLoader = new UnityDictionaryLoader();

        public static readonly EngDict EngDict = new EngDict(dictionaryLoader);

        /// <inheritdoc />
        public override IReadOnlyList<WordDefinition> Invoke(string word)
        {
            var definitions = EngDict.Define(word);

            return definitions;
        }

        public void WarmUp()
        {
            EngDict.WarmUp();
        }
    }
}
