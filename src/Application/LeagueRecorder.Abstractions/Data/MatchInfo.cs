using System;

namespace LeagueRecorder.Abstractions.Data
{
    public class MatchInfo
    {
        public string Id { get; set; }

        public string GameId { get; set; }
        public Region Region { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.GameId, this.Region.GetAbbreviation());
        }

        protected bool Equals(MatchInfo other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MatchInfo) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}