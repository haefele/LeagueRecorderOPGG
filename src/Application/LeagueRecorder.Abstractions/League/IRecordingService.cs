using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Abstractions.League
{
    public interface IRecordingService
    {
        /// <summary>
        /// Asynchronously returns the current <see cref="MatchInfo"/> of the specified <paramref name="player"/>.
        /// </summary>
        /// <param name="player">The player.</param>
        Task<MatchInfo> GetCurrentMatchInfoFromPlayerAsync(Player player);
        /// <summary>
        /// Asynchronously requests to record the specified <paramref name="match"/>.
        /// Returns whether the match is beeing recorded.
        /// </summary>
        /// <param name="match">The match.</param>
        Task<bool> RequestRecordingOfMatchAsync(MatchInfo match);
    }
}