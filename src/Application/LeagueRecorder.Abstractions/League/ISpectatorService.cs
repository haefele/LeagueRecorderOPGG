using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Abstractions.League
{
    public interface ISpectatorService
    {
        /// <summary>
        /// Downloads the spectate-file and starts the League of Legends client to spectate the specified <paramref name="match"/>.
        /// </summary>
        /// <param name="match">The match.</param>
        Task<bool> SpectateMatchAsync(MatchInfo match);
    }
}