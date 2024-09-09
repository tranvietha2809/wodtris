using System;
using System.Collections.Generic;
using Wordtris.GameCore.Objects;
using UnityEngine;
using System.Linq;

namespace Wordtris.GameCore.Words
{
    public class RandomTetrominoWithLettersSource
    {
        private static readonly BlockType[] TetrominoTypes = (BlockType[])Enum.GetValues(typeof(BlockType));
        private LetterSource _currentLetterSource;

        private readonly BlockType _tetrominoType;
        private readonly Vector2Int[] _shapes;  // Change this to Vector2Int[]
        private readonly Dictionary<Vector2Int, char> _lettersByBlock;

        private readonly bool _isTetromino;

        public RandomTetrominoWithLettersSource(BlockType? blockType, LetterSource letterSource = null, bool isTetromino = true)
        {
            _currentLetterSource = letterSource ?? new RandomEngLetterSource();
            _isTetromino = isTetromino;

            // Create a random instance
            var random = new System.Random();

            // Filter out BlockType.Bomb from the array and store the remaining types
            var availableTypes = TetrominoTypes.Where(type => type != BlockType.Bomb).ToArray();

            // Select a random type from the filtered array
            _tetrominoType = blockType ??
                (isTetromino
                    ? availableTypes[random.Next(availableTypes.Length)]
                    : BlockType.Single);


            _shapes = Data.Cells[_tetrominoType];  // Assign directly to _shapes
            _lettersByBlock = GenerateLettersForBlocks();
        }

        private Dictionary<Vector2Int, char> GenerateLettersForBlocks()
        {
            var lettersByBlock = new Dictionary<Vector2Int, char>();

            foreach (var position in _shapes)
            {
                char letter = _currentLetterSource.Char;

                _currentLetterSource = _currentLetterSource.Next;

                lettersByBlock[position] = letter;
            }

            return lettersByBlock;
        }

        public List<ActiveChar> GenerateTetrominoChar(int startX, int startY)
        {
            var blocks = new List<ActiveChar>();

            foreach (var kvp in _lettersByBlock)
            {
                var position = kvp.Key;
                var letter = kvp.Value;
                var block = new ActiveChar(startX + position.x, startY + position.y, letter,
                    _tetrominoType.ToString());
                blocks.Add(block);
            }

            return blocks;
        }

        public List<ActiveChar> GenerateBombTetrominoChar(int startX, int startY)
        {
            var blocks = new List<ActiveChar>();

            foreach (var kvp in _lettersByBlock)
            {
                var position = kvp.Key;
                var letter = kvp.Value;
                var block = new ActiveChar(startX + position.x, startY + position.y, char.MinValue,
                    BlockType.Bomb.ToString());
                blocks.Add(block);
            }

            return blocks;
        }

        public BlockType GetTetrominoType() => _tetrominoType;

        public Vector2Int[] GetTetrominoShape() => _shapes;  // Updated return type

        public Dictionary<Vector2Int, char> GetLettersByBlock() => _lettersByBlock;

        public RandomTetrominoWithLettersSource Next(BlockType? blockType) => new RandomTetrominoWithLettersSource(blockType, _currentLetterSource, _isTetromino);
    }
}
