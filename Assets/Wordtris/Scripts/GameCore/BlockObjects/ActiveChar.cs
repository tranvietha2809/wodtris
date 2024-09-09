﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wordtris.GameCore.Objects
{
    /// <summary>
    /// Words controlled by a player.
    /// </summary>
    public class ActiveChar : WordisChar
    {
        public ActiveChar(
            int x,
            int y,
            char value,
            string groupId) : base(x, y, value, groupId)
        {
        }

        public override WordisObj Handle(
            WordisGame game,
            GameEvent gameEvent, List<ActiveChar> activeObjects)
        {
            if (gameEvent == GameEvent.Step)
            {
                return HandleStep(game);
            }

            if (gameEvent == GameEvent.Down)
            {
                return HandleDown(game);
            }

            if (gameEvent == GameEvent.Left)
            {
                return HandleLeft(game);
            }

            if (gameEvent == GameEvent.Right)
            {
                return HandleRight(game);
            }

            return this;
        }

        /// <summary>
        /// Moves the char down. Converts it to static on obstacle.
        /// </summary>
        private WordisObj HandleStep(WordisGame game)
        {
            // handle collision:
            if (game.Matrix[X, Y + 1] != null && !game.Matrix[X, Y + 1].Equals(this))
            {
                return new StaticChar(X, Y, Value, GroupId);
            }

            // on reaching water:
            if (game.Settings.HasWater && Y == game.Settings.AboveWaterY)
            {
                return new StaticChar(X, Y, Value, GroupId);
            }

            // on reaching the bottom:
            if (Y == game.Settings.MaxY)
            {
                return new StaticChar(X, Y, Value, GroupId);
            }

            var objectBelow = game.GameObjects
                .FirstOrDefault(o =>
                    o.X == X &&
                    o.Y == Y + 1);

            // on obstacle:
            if (objectBelow != null)
            {
                return new StaticChar(X, Y, Value, GroupId);
            }

            return With(y: Y + 1);
        }

        /// <summary>
        /// Moves the char down to the limit. Stops on obstacle.
        /// </summary>
        private WordisObj HandleDown(WordisGame game)
        {
            var firstObjBelow = game.GameObjects
                .Where(o =>
                    o.X == X &&
                    o.Y > Y)
                .OrderBy(o => o.Y)
                .FirstOrDefault();

            if (firstObjBelow == null)
            {
                if (game.Settings.HasWater)
                {
                    return With(y: game.Settings.AboveWaterY);
                }

                return With(y: game.Settings.MaxY);
            }

            // stop before obstacle
            return With(y: firstObjBelow.Y - 1);
        }

        /// <summary>
        /// Moves this char left.
        /// </summary>
        private WordisObj HandleLeft(WordisGame game)
        {
            var firstObjLeft = game.GameObjects
                .Where(o => o.Y == Y && o.X < X)
                .OrderByDescending(o => o.X)
                .FirstOrDefault();

            if (firstObjLeft?.X == X - 1 || X == 0)
            {
                // Stay in the current place
                return this;
            }

            return With(x: X - 1);
        }

        /// <summary>
        /// Moves this char right.
        /// </summary>
        private WordisObj HandleRight(WordisGame game)
        {
            var firstObjRight = game.GameObjects
                .Where(o => o.Y == Y && o.X > X)
                .OrderBy(o => o.X)
                .FirstOrDefault();

            if (firstObjRight?.X == X + 1 || X == game.Settings.MaxX)
            {
                // Stay in the current place
                return this;
            }

            return With(x: X + 1);
        }

        public WordisObj HandleStepCollisionCheck(WordisGame game)
        {
            // Check if the character is at the bottom or on an obstacle
            if (Y >= game.Settings.MaxY || game.Matrix[X, Y + 1] != null)
            {
                return new StaticChar(X, Y, Value, GroupId);
            }

            return this;
        }

        public ActiveChar With(int? x = null, int? y = null)
        {
            return new ActiveChar(
                x ?? X,
                y ?? Y,
                Value,
                GroupId);
        }
    }
}
