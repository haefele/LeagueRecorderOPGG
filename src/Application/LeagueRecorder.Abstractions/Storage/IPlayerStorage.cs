using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Abstractions.Storage
{
    public interface IPlayerStorage
    {
        /// <summary>
        /// Asynchronously returns all <see cref="Player"/>s.
        /// </summary>
        Task<IEnumerable<Player>> GetPlayersAsync();
        /// <summary>
        /// Asynchronously deletes the specified <paramref name="player"/>.
        /// </summary>
        /// <param name="player">The Player.</param>
        Task<bool> DeletePlayerAsync(Player player);
        /// <summary>
        /// Asynchronously adds the specified <paramref name="player"/>.
        /// </summary>
        /// <param name="player">The Player.</param>
        Task AddPlayerAsync(Player player);
    }
}