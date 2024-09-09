using System.Collections.Generic;

namespace Wordtris.GameCore.Objects
{
    public abstract class WordisObj
    {
        protected WordisObj(
            int x,
            int y,
            string groupId)
        {
            X = x;
            Y = y;
            GroupId = groupId;

        }

        public string GroupId { get; }

        public int X { get; }

        public int Y { get; }

        public (int x, int y) Point => (X, Y);

        public abstract WordisObj Handle(
            WordisGame wordisGame,
            GameEvent gameEvent, List<ActiveChar> activeObjects);

        #region Equality members

        protected bool Equals(WordisObj other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WordisObj)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        #endregion

        /// <summary>
        /// String representation for troubleshooting purpose.
        /// </summary>
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
