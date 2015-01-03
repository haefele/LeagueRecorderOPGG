using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Akavache;
using System.Reactive.Linq;
using Castle.Core;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.Storage;
using LiteGuard;

namespace LeagueRecorder.Windows.Storage
{
    public class UserStorage : IUserStorage
    {
        #region Fields
        private readonly IBlobCache _blobCache;
        private readonly IIdentityGenerator _identityGenerator;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UserStorage" /> class.
        /// </summary>
        /// <param name="blobCache">The BLOB cache.</param>
        /// <param name="identityGenerator">The identity generator.</param>
        public UserStorage(IBlobCache blobCache, IIdentityGenerator identityGenerator)
        {
            Guard.AgainstNullArgument("blobCache", blobCache);
            Guard.AgainstNullArgument("identityGenerator", identityGenerator);

            this._blobCache = blobCache;
            this._identityGenerator = identityGenerator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Asynchronously returns all <see cref="User"/>s.
        /// </summary>
        public Task<IEnumerable<User>> GetUsersAsync()
        {
            return this._blobCache.GetAllObjects<User>().ToTask();
        }
        /// <summary>
        /// Asynchronously deletes the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        public async Task<bool> DeleteUserAsync(User user)
        {
            Guard.AgainstNullArgument("user", user);

            User loadedUser = await this._blobCache.GetObject<User>(user.Id).ToTask().ConfigureAwait(false);

            if (loadedUser == null)
                return false;

            await this._blobCache.InvalidateObject<User>(user.Id).ToTask().ConfigureAwait(false);
            return true;
        }
        /// <summary>
        /// Asynchronously adds the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        public async Task AddUserAsync(User user)
        {
            Guard.AgainstNullArgument("user", user);

            if (user.Id != null)
                throw new InvalidOperationException("The user already has an ID.");

            user.Id = this._identityGenerator.Generate();
            await this._blobCache.InsertObject(user.Id, user).ToTask().ConfigureAwait(false);
        }
        #endregion
    }
}