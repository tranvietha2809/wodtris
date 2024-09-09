using System;
using System.Collections.Generic;
using Wordtris.GameCore.Objects;
using Newtonsoft.Json;

namespace Wordtris.GameCore
{
    /// <summary>
    /// Extension for essential <see cref="WordMatch"/>.
    /// </summary>
    public class WordMatchEx : WordMatch
    {
        [JsonConstructor]
        private WordMatchEx(
            IEnumerable<WordisChar> chars,
            int gameEventId,
            DateTimeOffset timestamp)
            : base(chars)
        {
            GameEventId = gameEventId;
            Timestamp = timestamp;
        }

        public WordMatchEx(
            WordMatch wordMatch,
            int gameEventId,
            DateTimeOffset timestamp)
            : this(
                wordMatch.MatchedChars,
                gameEventId,
                timestamp)
        {
        }

        /// <summary>
        /// Zero-based event id (when the match occured).
        /// </summary>
        public int GameEventId { get; }

        /// <summary>
        /// Match timestamp.
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        protected bool Equals(WordMatchEx other)
        {
            return (ReferenceEquals(this, other));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WordMatchEx)obj);
        }

        public override int GetHashCode()
        {
            return MatchedChars.GetHashCode();
        }
    }
}
