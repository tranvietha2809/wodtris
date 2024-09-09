using System.Collections.Generic;
using Wordtris.GameCore.Functions.Dictionary;

namespace Wordtris.GameCore.Functions
{
    /// <summary>
    /// Gets definitions for the given word.
    /// </summary>
    public abstract class DefineWordFunc
    {
        /// <summary>
        /// Invokes the function.
        /// </summary>
        /// <param name="word">Target word.</param>
        /// <returns>Definitions of the word.</returns>
        public abstract IReadOnlyList<WordDefinition> Invoke(string word);
    }
}
