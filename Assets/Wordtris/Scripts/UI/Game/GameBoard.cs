﻿using System.Collections;
using System.Collections.Generic;
using Wordtris.GameCore;
using Assets.Wordis.Frameworks.Utils;
using UnityEngine;

namespace Wordtris.Scripts.GamePlay
{
    /// <summary>
    /// Represents game board UI
    /// </summary>
    public class GameBoard : Singleton<GameBoard>
    {
        [Header("Public Class Members")]
        [Tooltip("BoardGenerator Script Reference")]
        public BoardGenerator boardGenerator;

        [Header("Other Public Members")]

        //List of all Blocks in Row X Column format.
        [System.NonSerialized]
        public List<List<Block>> allRows = new List<List<Block>>();

        [System.NonSerialized]
        public List<List<Block>> allColumns = new List<List<Block>>();

        /// <summary>
        /// Will get called when board grid gets initialized.
        /// </summary>
        public void OnBoardGridReady()
        {
            int totalRows = allRows.Count;

            for (int rowId = 0; rowId < allRows[0].Count; rowId++)
            {
                var thisColumn = new List<Block>();
                for (int columnId = 0; columnId < totalRows; columnId++)
                {
                    thisColumn.Add(allRows[columnId][rowId]);
                }

                allColumns.Add(thisColumn);
            }
        }

        /// <summary>
        /// Clears all given blocks from the board. On Completion state of block will be empty.
        /// </summary>
        public static IEnumerator ClearAllBlocks(WordisSettings settings = null, params Block[] allBlocks)
        {
            //Below calculation is done so blocks starts clearing from center to end on both sides.
            int middleIndex = allBlocks.Length % 2 == 0
                ? allBlocks.Length / 2
                : allBlocks.Length / 2 + 1; // todo: 3 -> 2 suspicious logic?

            int leftIndex = middleIndex - 1;
            int rightIndex = middleIndex;
            int totalBlocks = allBlocks.Length;

            for (int i = 0; i < middleIndex; i++, leftIndex--, rightIndex++)
            {
                if (leftIndex >= 0)
                {
                    allBlocks[leftIndex].Clear(settings);
                }

                if (rightIndex < totalBlocks)
                {
                    allBlocks[rightIndex].Clear(settings);
                }

                yield return new WaitForSeconds(0.03F);
            }

            yield return 0;
        }

        /// <summary>
        /// Reset the game. All the data, grid, and all UI will reset as fresh game.
        /// </summary>
        public void Clear() => boardGenerator.Clear();

        public Block GetBlockAtPosition(int x, int y)
        {
            // Implement this method to return the Block object at the specified grid position
            // This would typically involve accessing a 2D array or list that holds the Blocks
            return allColumns[x][y];
        }
    }
}