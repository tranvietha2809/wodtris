using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wordtris.GameCore.Objects
{
    /// <summary>
    /// Manages a group of ActiveChar blocks representing a Tetromino.
    /// </summary>
    public class TetrominoChar
    {
        public BlockType TetrominoType { get; private set; }
        public List<ActiveChar> Blocks { get; private set; } // List of blocks in the Tetromino

        public bool isLock = false;

        private int currentRotationState = 0; // Track the current rotation state

        public TetrominoChar(BlockType tetrominoType, List<ActiveChar> blocks)
        {
            this.TetrominoType = tetrominoType;
            this.Blocks = blocks;
        }
        public void Clear()
        {
            Blocks.Clear();
        }

        public List<WordisObj> Handle(WordisGame game, GameEvent gameEvent)
        {
            List<WordisObj> wordisObjs = new List<WordisObj>();

            if (gameEvent == GameEvent.Step)
            {
                if (!HandleStep(game))
                {
                    wordisObjs.AddRange(Lock(game));
                    return wordisObjs;
                }
            }
            else if (gameEvent == GameEvent.Down)
            {
                HandleDown(game);
                wordisObjs.AddRange(Lock(game));
                return wordisObjs;
            }
            else if (gameEvent == GameEvent.Left)
            {
                HandleLeft(game);
            }
            else if (gameEvent == GameEvent.Right)
            {
                HandleRight(game);
            }
            else if (gameEvent == GameEvent.RotateRight && TetrominoType != BlockType.Single)
            {
                Rotate(game, -1);
            }


            wordisObjs.AddRange(Blocks);

            return wordisObjs;
        }

        private bool HandleStep(WordisGame game)
        {
            if (CanMove(game, Vector2Int.up))
            {
                Blocks = Blocks.Select(block => block.With(y: block.Y + 1)).ToList();
                return true;
            }
            return false;
        }

        private void HandleDown(WordisGame game)
        {
            while (CanMove(game, Vector2Int.up))
            {
                Blocks = Blocks.Select(block => block.With(y: block.Y + 1)).ToList();
            }
        }

        private void HandleLeft(WordisGame game)
        {
            if (CanMove(game, Vector2Int.left))
            {
                Blocks = Blocks.Select(block => block.With(x: block.X - 1)).ToList();
            }
        }

        private void HandleRight(WordisGame game)
        {
            if (CanMove(game, Vector2Int.right))
            {
                Blocks = Blocks.Select(block => block.With(x: block.X + 1)).ToList();
            }
        }

        private void Rotate(WordisGame game, int rotationStep)
        {
            // Update the current rotation state (0, 1, 2, 3) corresponding to 0, 90, 180, 270 degrees
            currentRotationState = (currentRotationState + rotationStep + 4) % 4;

            Vector2Int[] rotatedPositions = new Vector2Int[Blocks.Count];
            Vector2Int pivot = GetPivot();

            // Apply the rotation matrix to each block
            int[,] rotationMatrix = GetRotationMatrix(currentRotationState);
            for (int i = 0; i < Blocks.Count; i++)
            {
                Vector2Int relativePos = new Vector2Int(Blocks[i].X - pivot.x, Blocks[i].Y - pivot.y);
                int newX = Mathf.RoundToInt(pivot.x + (relativePos.x * rotationMatrix[0, 0] + relativePos.y * rotationMatrix[0, 1]));
                int newY = Mathf.RoundToInt(pivot.y + (relativePos.x * rotationMatrix[1, 0] + relativePos.y * rotationMatrix[1, 1]));

                rotatedPositions[i] = new Vector2Int(newX, newY);
            }

            // Attempt the rotation, applying wall kicks if necessary
            if (!ApplyWallKick(game, rotatedPositions, currentRotationState, rotationStep))
            {
                return; // If the rotation with wall kicks fails, don't update the block positions.
            }

            // Update the block positions
            Blocks = Blocks.Select((block, i) => block.With(x: rotatedPositions[i].x, y: rotatedPositions[i].y)).ToList();
        }

        private bool ApplyWallKick(WordisGame game, Vector2Int[] rotatedPositions, int currentRotationState, int rotationStep)
        {
            Vector2Int[,] wallKickOffsets = GetWallKickData(TetrominoType, currentRotationState, rotationStep);

            // Try each wall kick offset to see if the rotation can be valid
            for (int i = 0; i < wallKickOffsets.GetLength(1); i++)
            {
                Vector2Int offset = wallKickOffsets[currentRotationState, i];
                if (IsValidRotation(game, rotatedPositions, offset))
                {
                    // Apply the offset to the rotated positions
                    for (int j = 0; j < rotatedPositions.Length; j++)
                    {
                        rotatedPositions[j] += offset;
                    }
                    return true; // Rotation is successful with this wall kick
                }
            }

            return false; // No valid rotation found with wall kicks
        }

        private Vector2Int[,] GetWallKickData(BlockType tetrominoType, int currentRotationState, int rotationStep)
        {
            return Data.WallKicks[tetrominoType];
        }

        private bool IsValidRotation(WordisGame game, Vector2Int[] rotatedPositions, Vector2Int offset)
        {
            foreach (var pos in rotatedPositions)
            {
                int x = pos.x + offset.x;
                int y = pos.y + offset.y;

                if (!IsWithinBounds(game, x, y) || (game.Matrix[x, y] != null && Blocks.FirstOrDefault(obj => obj.X == x && obj.Y == y) == null))
                {
                    return false;
                }
            }
            return true;
        }

        private int[,] GetRotationMatrix(int rotationState)
        {
            switch (rotationState)
            {
                case 0: // 0 degrees
                    return new int[,] { { 1, 0 }, { 0, 1 } };
                case 1: // 90 degrees clockwise
                    return new int[,] { { 0, 1 }, { -1, 0 } };
                case 2: // 180 degrees
                    return new int[,] { { -1, 0 }, { 0, -1 } };
                case 3: // 270 degrees clockwise
                    return new int[,] { { 0, -1 }, { 1, 0 } };
                default:
                    return new int[,] { { 1, 0 }, { 0, 1 } }; // Default is 0 degrees, though this should never be hit.
            }
        }

        private Vector2Int GetPivot()
        {
            // Calculate the bounding box of the Tetromino
            int minX = Blocks.Min(block => block.X);
            int maxX = Blocks.Max(block => block.X);
            int minY = Blocks.Min(block => block.Y);
            int maxY = Blocks.Max(block => block.Y);

            // Calculate the center of the bounding box
            int pivotX = Mathf.RoundToInt((minX + maxX) / 2f);
            int pivotY = Mathf.RoundToInt((minY + maxY) / 2f);

            return new Vector2Int(pivotX, pivotY);
        }

        private bool CanMove(WordisGame game, Vector2Int direction)
        {
            foreach (var block in Blocks)
            {
                int newX = block.X + direction.x;
                int newY = block.Y + direction.y;
                Debug.Log(block.Value.ToString() + " " + newX + " " + newY + (!IsWithinBounds(game, newX, newY) || (game.Matrix[newX, newY] != null && Blocks.FirstOrDefault(obj => obj.X == newX && obj.Y == newY) == null)));

                if (!IsWithinBounds(game, newX, newY) || (game.Matrix[newX, newY] != null && Blocks.FirstOrDefault(obj => obj.X == newX && obj.Y == newY) == null))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsWithinBounds(WordisGame game, int x, int y)
        {
            return x >= 0 && x < game.Settings.MaxX + 1 && y >= 0 && y < game.Settings.MaxY + 1;
        }

        private List<StaticChar> Lock(WordisGame game)
        {
            List<StaticChar> wordisObjs = new List<StaticChar>();

            foreach (var block in Blocks)
            {
                wordisObjs.Add(new StaticChar(block.X, block.Y, block.Value, block.GroupId));
            }
            isLock = true;
            return wordisObjs;
        }
    }
}
