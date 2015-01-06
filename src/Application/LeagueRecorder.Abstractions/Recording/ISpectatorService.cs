using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;
using NeverNull;

namespace LeagueRecorder.Abstractions.Recording
{
    public interface ISpectatorService
    {
        Task<bool> SpectateMatchAsync(MatchInfo match);
    }
}