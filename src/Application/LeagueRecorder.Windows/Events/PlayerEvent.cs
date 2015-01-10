using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Windows.Events
{
    public abstract class PlayerEvent
    {
        protected PlayerEvent(Player player)
        {
            this.Player = player;
        }

        public Player Player { get; private set; }
    }
}