using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Windows.Events
{
    public class PlayerRemovedEvent : PlayerEvent
    {
        public PlayerRemovedEvent(Player player) 
            : base(player)
        {
        }
    }
}