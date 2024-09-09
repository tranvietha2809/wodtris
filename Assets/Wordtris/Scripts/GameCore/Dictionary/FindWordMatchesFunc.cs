using System.Collections.Generic;
using System.Linq;
using Wordtris.GameCore.Objects;

namespace Wordtris.GameCore.Functions
{
    /// <summary>
    /// Finds word matches among the <see cref="StaticChar"/> objects on the board.
    /// </summary>
    public class FindWordMatchesFunc
    {
        // A reference to the function that checks if a word is legitimate.
        private readonly IsLegitWordFunc _isLegitWordFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindWordMatchesFunc"/> class.
        /// </summary>
        /// <param name="isLegitWordFunc">Optional custom function to check if a word is legitimate. 
        /// If not provided, a default English word validator is used.</param>
        public FindWordMatchesFunc(IsLegitWordFunc isLegitWordFunc = null)
        {
            // If no custom function is provided, use a default implementation for English words.
            _isLegitWordFunc = isLegitWordFunc ?? new IsLegitEngWordFunc();
        }

        /// <summary>
        /// Finds words in the specified direction (horizontal or vertical) within the matrix of Wordtris objects.
        /// </summary>
        /// <param name="matrix">Matrix of Wordtris objects</param>
        /// <param name="minWordLength">Minimum acceptable word length</param>
        /// <param name="horizontal">If true, search horizontally; otherwise, search vertically.</param>
        /// <param name="matches">List to store the found word matches</param>
        private void FindMatchesInDirection(WordisMatrix matrix, int minWordLength, bool horizontal, List<WordMatch> matches)
        {
            // List to keep track of the current sequence of StaticChars that could form a word.
            var currentChars = new List<StaticChar>();

            // Determine the primary and secondary dimensions based on the search direction.
            int primaryDimension = horizontal ? matrix.Height : matrix.Width;
            int secondaryDimension = horizontal ? matrix.Width : matrix.Height;

            // Determine the search limit to avoid checking sequences shorter than the minimum word length.
            int searchLimit = secondaryDimension - minWordLength + 1;

            // Loop through each row (horizontal) or column (vertical) depending on the search direction.
            for (int primaryIndex = 0; primaryIndex < primaryDimension; primaryIndex++)
            {
                int secondaryIndex = 0;

                // Loop through each character in the current row or column.
                while (secondaryIndex < searchLimit)
                {
                    int tempIndex = secondaryIndex;

                    // Accumulate characters in the current sequence until a non-character object is found or the end is reached.
                    while (tempIndex < secondaryDimension)
                    {
                        WordisObj matrixElement = horizontal ? matrix[tempIndex, primaryIndex] : matrix[primaryIndex, tempIndex];

                        // Check if the current element is a StaticChar.
                        if (matrixElement is StaticChar staticChar)
                        {
                            currentChars.Add(staticChar);
                            tempIndex++;
                        }
                        else
                        {
                            break; // Stop accumulating if a non-character is found.
                        }

                        // Check if the accumulated characters form a valid word.
                        if (currentChars.Count >= minWordLength)
                        {
                            string word = new string(currentChars.Select(c => c.Value).ToArray());

                            // If the word is valid, add it to the matches list.
                            if (_isLegitWordFunc.Invoke(word))
                            {
                                matches.Add(new WordMatch(currentChars));
                            }
                        }
                    }

                    // If the current sequence is too short, skip to the next possible start position.
                    if (currentChars.Count <= minWordLength)
                    {
                        for (secondaryIndex = tempIndex + 1; secondaryIndex < searchLimit; secondaryIndex++)
                        {
                            if ((horizontal ? matrix[secondaryIndex, primaryIndex] : matrix[primaryIndex, secondaryIndex]) is StaticChar)
                            {
                                break; // Start a new sequence at the next character.
                            }
                        }
                    }
                    else
                    {
                        secondaryIndex++; // Move to the next possible start position.
                    }

                    // Clear the current character sequence to start fresh.
                    currentChars.Clear();
                }
            }
        }

        /// <summary>
        /// Finds English words in a matrix of Wordtris objects.
        /// </summary>
        /// <param name="matrix">Matrix of Wordtris objects</param>
        /// <param name="minWordLength">Minimum acceptable word length</param>
        /// <returns>Array of found word matches</returns>
        public virtual WordMatch[] Invoke(WordisMatrix matrix, int minWordLength)
        {
            // List to store all found word matches.
            var matches = new List<WordMatch>();

            // Find matches in horizontal direction.
            FindMatchesInDirection(matrix, minWordLength, true, matches);

            // Find matches in vertical direction.
            FindMatchesInDirection(matrix, minWordLength, false, matches);

            // Return all found matches as an array.
            return matches.ToArray();
        }
    }
}
