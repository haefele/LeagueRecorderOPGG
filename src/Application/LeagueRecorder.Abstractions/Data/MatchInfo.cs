namespace LeagueRecorder.Abstractions.Data
{
    public class MatchInfo
    {
        public MatchInfo(string gameId, Region region)
        {
            this.GameId = gameId;
            this.Region = region;
        }

        public string GameId { get; private set; }
        public Region Region { get; private set; }
    }
}