using System;
using System.Collections.Generic;
using Wordtris.Scripts.UI.Extensions;
using Assets.Wordis.Frameworks.ThemeManager.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Wordtris.GameCore;

namespace Wordtris.Scripts.GamePlay
{
    /// <summary>
    /// This script component will generate the board with given size and will also place blocks from previous session if there is progress.
    /// </summary>
    public class BoardGenerator : MonoBehaviour
    {
#pragma warning disable 0649
        // Prefab template of block.
        [SerializeField] public GameObject blockTemplate;

        // Parent inside which all blocks will be generated. Typically root of block grid.
        [SerializeField] public GameObject blockRoot;

        [SerializeField] public float blockSize = 90;

        [SerializeField] public float blockSpace = 3;
#pragma warning restore 0649

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
        }

        /// <summary>
        /// Generates the block grid based on game settings and will also set progress from previous session if any.
        /// </summary>
        public void GenerateBoard(WordisSettings wordisSettings)
        {
            // TODO: check this buggy code
            int rowSize = wordisSettings.Height;
            int columnSize = wordisSettings.Width;
            Debug.Log("rowSize" + rowSize);

            // Starting points represents point from where block shape grid should start inside block shape.
            float startPointX = GetStartPointX(blockSize, columnSize);
            float startPointY = GetStartPointY(blockSize, rowSize);

            // Will keep updating with iterations.
            float currentPositionX = startPointX;
            float currentPositionY = startPointY;

            GamePlayUI.Instance.gameBoard.allRows = new List<List<Block>>();
            GamePlayUI.Instance.gameBoard.allColumns = new List<List<Block>>();

            // Iterates through all rows and columns to generate grid.
            for (int row = 0; row < rowSize; row++)
            {
                List<Block> blockRow = new List<Block>();

                for (int column = 0; column < columnSize; column++)
                {
                    // Spawn a block instance and prepares it.
                    Block block = SpawnBlock(currentPositionX, currentPositionY, blockSize, row, column);

                    if (wordisSettings.IsWaterZone(row))
                    {
                        block.PlaceBlock(Block.WaterTag);
                    }

                    blockRow.Add(block);

                    currentPositionX += blockSize + blockSpace;
                }

                currentPositionX = startPointX;
                currentPositionY -= blockSize + blockSpace;

                GamePlayUI.Instance.gameBoard.allRows.Add(blockRow);
            }

            GameBoard.Instance.OnBoardGridReady();
        }

        /// <summary>
        /// Resets Grid and removes all blocks from it.
        /// </summary>
        public void Clear() => blockRoot.ClearAllChild();

        /// <summary>
        /// Horizontal position from where block grid will start.
        /// </summary>
        private float GetStartPointX(float blockSize, int rowSize)
        {
            float totalWidth =
                blockSize * rowSize +
                (rowSize - 1) * blockSpace;
            return -(totalWidth / 2 - blockSize / 2);
        }

        /// <summary>
        /// Vertical position from where block grid will start.
        /// </summary>
        private float GetStartPointY(float blockSize, int columnSize)
        {
            float totalHeight =
                blockSize * columnSize +
                (columnSize - 1) * blockSpace;
            return totalHeight / 2 - blockSize / 2;
        }

        /// <summary>
        /// Spawns a new block.
        /// </summary>
        private Block SpawnBlock(
            float currentPositionX,
            float currentPositionY,
            float blockSize,
            int row,
            int column)
        {
            Sprite blockBgSprite =
                ThemeManager.Instance.GetBlockSpriteWithTag(
                    blockTemplate.GetComponent<Block>().defaultSpriteTag);

            RectTransform blockElement = GetBlockInsideGrid();
            blockElement.localPosition = new Vector3(
                x: currentPositionX,
                y: currentPositionY,
                z: 0);

            blockElement.sizeDelta = Vector3.one * blockSize;
            blockElement.GetComponent<BoxCollider2D>().size = Vector3.one * blockSize;
            blockElement.GetComponent<Image>().sprite = blockBgSprite;
            blockElement.name = $"block-{row}{column}";

            // Sets blocks logical position inside grid and its default sprite.
            Block block = blockElement.GetComponent<Block>();
            block.gameObject.SetActive(true);
            block.SetBlockLocation(row, column);
            block.assignedSpriteTag = block.defaultSpriteTag;
            return block;
        }

        /// <summary>
        /// Spawn a new block instance and sets its block root as its parent.
        /// </summary>
        private RectTransform GetBlockInsideGrid()
        {
            GameObject block = Instantiate(blockTemplate);
            block.transform.SetParent(blockRoot.transform);
            block.transform.localScale = Vector3.one;

            return block.GetComponent<RectTransform>();
        }
    }
}