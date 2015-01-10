using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Windows.Events
{
    public class PlayerAddedEvent : PlayerEvent
    {
        public PlayerAddedEvent(Player player) 
            : base(player)
        {
        }
    }
}