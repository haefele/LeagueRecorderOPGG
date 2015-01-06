using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Castle.Core;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.Storage;
using LiteGuard;
using Xemio.CommonLibrary.Storage;

namespace LeagueRecorder.Windows.Storage
{
    public class PlayerStorage : IPlayerStorage, IStartable
    {
        #region Fields
        private readonly IDataStorage _dataStorage;
        private readonly IIdentityGenerator _identityGenerator;

        private List<Player> _cachedPlayers; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerStorage" /> class.
        /// </summary>
        /// <param name="dataStorage">The data storage.</param>
        /// <param name="identityGenerator">The identity generator.</param>
        public PlayerStorage(IDataStorage dataStorage, IIdentityGenerator identityGenerator)
        {
            Guard.AgainstNullArgument("dataStorage", dataStorage);
            Guard.AgainstNullArgument("identityGenerator", identityGenerator);

            this._dataStorage = dataStorage;
            this._identityGenerator = identityGenerator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Asynchronously returns all <see cref="Player"/>s.
        /// </summary>
        public Task<IEnumerable<Player>> GetPlayersAsync()
        {
            IEnumerable<Player> result = new List<Player>(this._cachedPlayers);
            return Task.FromResult(result);
        }
        /// <summary>
        /// Asynchronously deletes the specified <paramref name="player"/>.
        /// </summary>
        /// <param name="player">The Player.</param>
        public Task<bool> DeletePlayerAsync(Player player)
        {
            Guard.AgainstNullArgument("player", player);
            Guard.AgainstNullArgumentProperty("player", "Id", player.Id);

            Player foundPlayer = this._cachedPlayers.FirstOrDefault(f => f.Id == player.Id);

            if (foundPlayer == null)
                return Task.FromResult(false);

            this._cachedPlayers.Remove(foundPlayer);

            return Task.FromResult(true);
        }
        /// <summary>
        /// Asynchronously adds the specified <paramref name="player"/>.
        /// </summary>
        /// <param name="player">The player.</param>
        public Task AddPlayerAsync(Player player)
        {
            Guard.AgainstNullArgument("player", player);

            if (player.Id != null)
                throw new InvalidOperationException("The Player already has an ID.");

            player.Id = this._identityGenerator.Generate();
            this._cachedPlayers.Add(player);

            return Task.FromResult(new object());
        }
        #endregion

        #region Implementation of IStartable
        /// <summary>
        /// Starts this instance.
        /// </summary>
        void IStartable.Start()
        {
            this._cachedPlayers = this._dataStorage.Retrieve<List<Player>>() ?? new List<Player>();
        }
        /// <summary>
        /// Stops this instance.
        /// </summary>
        void IStartable.Stop()
        {
            this._dataStorage.Store(this._cachedPlayers);
        }
        #endregion
    }
}