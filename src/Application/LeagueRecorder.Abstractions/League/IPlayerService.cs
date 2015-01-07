using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Abstractions.League
{
    public interface IPlayerService
    {
        /// <summary>
        /// Checks whether a player with the specified <paramref name="username"/> in the specified <paramref name="region"/> exists.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="region">The region.</param>
        Task<bool> PlayerExists(string username, Region region);
    }
}