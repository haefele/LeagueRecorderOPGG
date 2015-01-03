using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueRecorder.Abstractions.Data;

namespace LeagueRecorder.Abstractions.Storage
{
    public interface IUserStorage : IService
    {
        /// <summary>
        /// Asynchronously returns all <see cref="User"/>s.
        /// </summary>
        Task<IEnumerable<User>> GetUsersAsync();
        /// <summary>
        /// Asynchronously deletes the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        Task<bool> DeleteUserAsync(User user);
        /// <summary>
        /// Asynchronously adds the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        Task AddUserAsync(User user);
    }
}