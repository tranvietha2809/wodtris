using System;
using System.Collections.Generic;
using System.Linq;

namespace Wordtris.GameCore.Functions.Dictionary
{
    /// <summary>
    /// Interface for loading dictionary content.
    /// This abstracts the data source and removes the Unity dependency.
    /// </summary>
    public interface IDictionaryLoader
    {
        /// <summary>
        /// Loads the content of the dictionary file based on the root letter.
        /// </summary>
        /// <param name="rootLetter">The letter that indicates which dictionary file to load.</param>
        /// <returns>A string containing the raw dictionary data.</returns>
        string LoadDictionaryContent(string rootLetter);
    }

    /// <summary>
    /// Base functionality for language dictionary.
    /// This class handles the storage and retrieval of word definitions.
    /// </summary>
    public abstract class DictionaryBase
    {
        // Lazy-loaded dictionary to store word definitions. Lazy loading ensures that the dictionary is only created when needed.
        private readonly Lazy<IDictionary<string, WordDefinition[]>> _allDefinitions;

        // Reference to a dictionary loader instance, which is responsible for loading the dictionary data.
        private readonly IDictionaryLoader _dictionaryLoader;

        /// <summary>
        /// Initializes a new instance of the DictionaryBase class.
        /// </summary>
        /// <param name="dictionaryLoader">An instance of IDictionaryLoader for loading dictionary content.</param>
        protected DictionaryBase(IDictionaryLoader dictionaryLoader)
        {
            // Ensure the loader is not null.
            _dictionaryLoader = dictionaryLoader ?? throw new ArgumentNullException(nameof(dictionaryLoader));

            // Initialize the lazy-loaded dictionary, which will load and parse the dictionary content when accessed.
            _allDefinitions = new Lazy<IDictionary<string, WordDefinition[]>>(() =>
                GetWordDefinitions(RootLetter));
        }

        /// <summary>
        /// The root letter used to determine which dictionary file to load.
        /// Subclasses can override this to specify different root letters.
        /// </summary>
        protected virtual string RootLetter => string.Empty;

        /// <summary>
        /// Retrieves the definitions for a given word.
        /// </summary>
        /// <param name="word">The word to look up.</param>
        /// <returns>A list of definitions for the word, or an empty list if the word is not found.</returns>
        public virtual IReadOnlyList<WordDefinition> Define(string word)
        {
            // Attempt to find the word in the dictionary.
            var isWordDefined = _allDefinitions.Value.TryGetValue(
                word,
                out var wordDefinitions);

            // Return the definitions if found, otherwise return an empty array.
            return isWordDefined
                ? wordDefinitions
                : Array.Empty<WordDefinition>();
        }

        /// <summary>
        /// Loads and parses the dictionary content based on the root letter.
        /// </summary>
        /// <param name="rootLetter">The letter indicating which dictionary file to load.</param>
        /// <returns>A dictionary mapping words to their definitions.</returns>
        private Dictionary<string, WordDefinition[]> GetWordDefinitions(string rootLetter)
        {
            // Use the dictionary loader to get the raw content of the dictionary file.
            var rawContent = _dictionaryLoader.LoadDictionaryContent(rootLetter);

            // Parse the raw content into word definitions, grouping by word.
            var definitions = rawContent
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .AsParallel() // Parallel processing to speed up parsing.
                .Select(WordDefinition.Parse)
                .GroupBy(d => d.Word, StringComparer.OrdinalIgnoreCase) // Group by word (case-insensitive).
                .ToDictionary(g =>
                    g.Key, // Word as the key.
                    g => g.Take(1).ToArray(), // Take only the first definition for now.
                    StringComparer.OrdinalIgnoreCase); // Case-insensitive dictionary.

            return definitions; // Return the dictionary of words and definitions.
        }
    }
}
