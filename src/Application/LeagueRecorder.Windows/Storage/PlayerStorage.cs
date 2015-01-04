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
    public class PlayerStorage : IPlayerStorage
    {
        #region Fields
        private readonly IBlobCache _blobCache;
        private readonly IIdentityGenerator _identityGenerator;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerStorage" /> class.
        /// </summary>
        /// <param name="blobCache">The BLOB cache.</param>
        /// <param name="identityGenerator">The identity generator.</param>
        public PlayerStorage(IBlobCache blobCache, IIdentityGenerator identityGenerator)
        {
            Guard.AgainstNullArgument("blobCache", blobCache);
            Guard.AgainstNullArgument("identityGenerator", identityGenerator);

            this._blobCache = blobCache;
            this._identityGenerator = identityGenerator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Asynchronously returns all <see cref="Player"/>s.
        /// </summary>
        public Task<IEnumerable<Player>> GetPlayersAsync()
        {
            return this._blobCache.GetAllObjects<Player>().ToTask();
        }
        /// <summary>
        /// Asynchronously deletes the specified <paramref name="player"/>.
        /// </summary>
        /// <param name="player">The Player.</param>
        public async Task<bool> DeletePlayerAsync(Player player)
        {
            Guard.AgainstNullArgument("player", player);
            Guard.AgainstNullArgumentProperty("player", "Id", player.Id);

            Player loadedPlayer = await this._blobCache.GetObject<Player>(player.Id).ToTask().ConfigureAwait(false);

            if (loadedPlayer == null)
                return false;

            await this._blobCache.InvalidateObject<Player>(player.Id).ToTask().ConfigureAwait(false);
            return true;
        }
        /// <summary>
        /// Asynchronously adds the specified <paramref name="player"/>.
        /// </summary>
        /// <param name="player">The player.</param>
        public async Task AddPlayerAsync(Player player)
        {
            Guard.AgainstNullArgument("player", player);

            if (player.Id != null)
                throw new InvalidOperationException("The Player already has an ID.");

            player.Id = this._identityGenerator.Generate();
            await this._blobCache.InsertObject(player.Id, player).ToTask().ConfigureAwait(false);
        }
        #endregion
    }
}