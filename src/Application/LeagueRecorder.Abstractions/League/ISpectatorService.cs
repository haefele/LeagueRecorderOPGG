using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Abstractions.League
{
    public interface ISpectatorService
    {
        Task<bool> SpectateMatchAsync(MatchInfo match);
    }
}