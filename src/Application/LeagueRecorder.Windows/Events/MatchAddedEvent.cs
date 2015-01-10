using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Windows.Events
{
    public class MatchAddedEvent
    {
        public MatchAddedEvent(MatchInfo match)
        {
            this.Match = match;
        }

        public MatchInfo Match { get; private set; }
    }
}