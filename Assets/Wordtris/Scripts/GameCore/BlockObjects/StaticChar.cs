using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wordtris.GameCore.Objects
{
    /// <summary>
    /// Represents a character that has found its place and is no longer controlled by the player.
    /// </summary>
    public class StaticChar : WordisChar
    {
        public StaticChar(
            int x,
            int y,
            char value,
            string groupId) : base(x: x, y: y, value: value, groupId: groupId)
        {
        }

        public override WordisObj Handle(WordisGame game, GameEvent gameEvent, List<ActiveChar> activeObjects)
        {
            if (gameEvent == GameEvent.Step)
            {
                if (game.Settings.HasWater)
                {
                    var staticCharsAbove = 0;
                    var sparePlaceAbove = 0;
                    var sparePlaceBelow = 0;

                    for (int y = 0; y < game.Settings.Height; y++)
                    {
                        if (game.Matrix[X, y] is StaticChar && y < Y)
                        {
                            staticCharsAbove++;
                        }
                        if (game.Matrix[X, y] == null && y < Y)
                        {
                            sparePlaceAbove++;
                        }
                        if (game.Matrix[X, y] == null && y > Y)
                        {
                            sparePlaceBelow++;
                        }
                    }

                    var destination = (x: X, y: game.Settings.AboveWaterY + staticCharsAbove);

                    if (destination.y > Y && sparePlaceBelow > 0)
                    {
                        // Move down by pressure if there's space below
                        return With(y: Y + 1);
                    }

                    if (destination.y < Y && sparePlaceAbove > 0)
                    {
                        // Emerge if there's space above
                        return With(y: Y - 1);
                    }

                    return this;
                }

                // Handle falling below in a grid with gravity
                var placeBelow = (x: X, y: Y + 1);

                if (game.Matrix[placeBelow.x, placeBelow.y] != null && game.Matrix[placeBelow.x, placeBelow.y].GroupId == GroupId)
                {
                    return this;
                }

                for (int y = placeBelow.y; y < game.Settings.Height; y++)
                {
                    if (game.Matrix[placeBelow.x, y] == null)
                    {
                        var placeNextRight = (x: X + 1, y: Y);
                        var placeNextLeft = (x: X - 1, y: Y);
                        if ((game.Matrix[placeNextRight.x, Y] != null && game.Matrix[placeNextRight.x, Y].GroupId == GroupId) || (game.Matrix[placeNextLeft.x, Y] != null && game.Matrix[placeNextLeft.x, Y].GroupId == GroupId))
                        {
                            return this;
                        }
                        return With(y: placeBelow.y);
                    }
                }

                return this;
            }

            // Static characters should not respond to other events
            return this;
        }

        public List<StaticChar> BombDestroyHandle(WordisGame game)
        {
            List<StaticChar> destroyedBlocks = new List<StaticChar>();


            if (isDynamite())
            {
                destroyedBlocks.Add(this);
                var belowPosition = new Vector2Int(X, Y + 1);
                if (game.Matrix[belowPosition.x, belowPosition.y] != null && game.Matrix[belowPosition.x, belowPosition.y] is StaticChar staticChar)
                {
                    destroyedBlocks.Add(staticChar);
                }
            }
            else
            if (isBomb())
            {
                destroyedBlocks.Add(this);
                var belowPosition = new Vector2Int(X, Y + 1);
                if (game.Matrix[belowPosition.x, belowPosition.y] != null && game.Matrix[belowPosition.x, belowPosition.y] is StaticChar staticChar)
                {
                    destroyedBlocks.Add(staticChar);
                }
            }
            return destroyedBlocks;
        }

        private StaticChar With(int? x = null, int? y = null, string? groupId = null)
        {
            return new StaticChar(
                x ?? X,
                y ?? Y,
                Value, groupId ?? GroupId);
        }
    }
}
