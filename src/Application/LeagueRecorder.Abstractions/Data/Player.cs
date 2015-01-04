using System;
using System.Globalization;

namespace LeagueRecorder.Abstractions.Data
{
    public class Player
    {
        public string Id { get; set; }

        public string Username { get; set; }
        public Region Region { get; set; }
        
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Username, this.Region.GetAbbreviation());
        }

        protected bool Equals(Player other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals((Player) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}