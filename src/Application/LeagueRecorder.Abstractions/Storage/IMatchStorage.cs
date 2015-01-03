using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Abstractions.Storage
{
    public interface IMatchStorage : IService
    {
        /// <summary>
        /// Asynchronously returns all <see cref="MatchInfo"/>s.
        /// </summary>
        Task<IEnumerable<MatchInfo>> GetMatchesAsync();
        /// <summary>
        /// Asynchronously adds the specified <paramref name="match"/>.
        /// </summary>
        /// <param name="match">The match.</param>
        Task AddMatchAsync(MatchInfo match);
    }
}