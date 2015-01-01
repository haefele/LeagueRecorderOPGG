using System.Globalization;

namespace LeagueRecorder.Abstractions.Data
{
    public class User
    {
        public string Username { get; set; }
        public Region Region { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Username, this.Region.GetAbbreviation());
        }
    }
}