using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Caliburn.Micro;
using Castle.Core;
using Castle.Core.Logging;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.Storage;
using LeagueRecorder.Windows.Events;
using LiteGuard;
using Xemio.CommonLibrary.Storage;

namespace LeagueRecorder.Windows.Storage
{
    public class PlayerStorage : IPlayerStorage, IStartable
    {
        #region Fields
        private readonly IDataStorage _dataStorage;
        private readonly IIdentityGenerator _identityGenerator;
        private readonly IEventAggregator _eventAggregator;

        private List<Player> _cachedPlayers; 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerStorage" /> class.
        /// </summary>
        /// <param name="dataStorage">The data storage.</param>
        /// <param name="identityGenerator">The identity generator.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public PlayerStorage(IDataStorage dataStorage, IIdentityGenerator identityGenerator, IEventAggregator eventAggregator)
        {
            Guard.AgainstNullArgument("dataStorage", dataStorage);
            Guard.AgainstNullArgument("identityGenerator", identityGenerator);
            Guard.AgainstNullArgument("eventAggregator", eventAggregator);

            this.Logger = NullLogger.Instance;

            this._dataStorage = dataStorage;
            this._identityGenerator = identityGenerator;
            this._eventAggregator = eventAggregator;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Asynchronously returns all <see cref="Player"/>s.
        /// </summary>
        public Task<IEnumerable<Player>> GetPlayersAsync()
        {
            this.Logger.DebugFormat("Requesting all players.");

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

            this.Logger.DebugFormat("Deleting the player: {0}, id: {1}", player, player.Id);

            Player foundPlayer = this._cachedPlayers.FirstOrDefault(f => f.Id == player.Id);

            if (foundPlayer == null)
            {
                this.Logger.DebugFormat("Could not find the player: {0}, id: {1}", player, player.Id);
                return Task.FromResult(false);
            }

            this._cachedPlayers.Remove(foundPlayer);

            this._eventAggregator.PublishOnUIThread(new PlayerRemovedEvent(foundPlayer));
            this.Logger.DebugFormat("Removed the player: {0}, id: {1}", player, player.Id);

            return Task.FromResult(true);
        }
        /// <summary>
        /// Asynchronously adds the specified <paramref name="player"/>.
        /// </summary>
        /// <param name="player">The player.</param>
        public Task AddPlayerAsync(Player player)
        {
            Guard.AgainstNullArgument("player", player);

            this.Logger.DebugFormat("Adding a new player: {0}", player);

            if (player.Id != null)
            {
                this.Logger.DebugFormat("Tried to store a player that is already stored. Id: {0}", player.Id);
                throw new InvalidOperationException("The Player already has an ID.");
            }

            player.Id = this._identityGenerator.Generate();
            this._cachedPlayers.Add(player);

            this._eventAggregator.PublishOnUIThread(new PlayerAddedEvent(player));
            this.Logger.DebugFormat("Stored a new player with id: {0}", player.Id);
            
            return Task.FromResult(new object());
        }
        #endregion

        #region Implementation of IStartable
        /// <summary>
        /// Starts this instance.
        /// </summary>
        void IStartable.Start()
        {
            this.Logger.DebugFormat("Loading all players from the data-storage.");
            this._cachedPlayers = this._dataStorage.Retrieve<List<Player>>() ?? new List<Player>();
        }
        /// <summary>
        /// Stops this instance.
        /// </summary>
        void IStartable.Stop()
        {
            this.Logger.DebugFormat("Saving all players in the data-storage.");
            this._dataStorage.Store(this._cachedPlayers);
        }
        #endregion
    }
}