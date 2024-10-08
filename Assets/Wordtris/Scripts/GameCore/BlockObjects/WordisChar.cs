﻿using System;

namespace Wordtris.GameCore.Objects
{
    /// <summary>
    /// Represents a single character.
    /// </summary>
    public abstract class WordisChar : WordisObj
    {
        protected WordisChar(
            int x,
            int y,
            char value, string groupId) : base(
            x: x,
            y: y, groupId: groupId)
        {
            Value = value;
        }

        #region Equality members

        public char Value { get; }

        protected bool Equals(WordisChar other) =>
            base.Equals(other) &&
            Value == other.Value;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WordisChar)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ Value.GetHashCode();
            }
        }

        #endregion

        /// <summary>
        /// String representation for troubleshooting purpose.
        /// </summary>
        public override string ToString()
        {
            return $"{Value},{base.ToString()}";
        }

        public bool isDynamite()
        {
            if (Enum.TryParse(GroupId, out BlockType tetrominoType))
            {
                return tetrominoType == BlockType.Dynamite;
            }
            return false;
        }

        public bool isBomb()
        {
            if (Enum.TryParse(GroupId, out BlockType tetrominoType))
            {
                return tetrominoType == BlockType.Bomb;
            }
            return false;
        }
    }
}
