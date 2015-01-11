using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Documents;
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
    public class MatchStorage : IMatchStorage, IStartable
    {
        #region Fields
        private readonly IDataStorage _dataStorage;
        private readonly IIdentityGenerator _identityGenerator;
        private readonly IEventAggregator _eventAggregator;

        private List<MatchInfo> _cachedMatches; 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchStorage"/> class.
        /// </summary>
        /// <param name="dataStorage">The data storage.</param>
        /// <param name="identityGenerator">The identity generator.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public MatchStorage(IDataStorage dataStorage, IIdentityGenerator identityGenerator, IEventAggregator eventAggregator)
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

        #region Implementation of IMatchStorage
        /// <summary>
        /// Asynchronously returns all <see cref="MatchInfo" />s.
        /// </summary>
        public Task<IEnumerable<MatchInfo>> GetMatchesAsync()
        {
            this.Logger.DebugFormat("Requesting all matches.");

            IEnumerable<MatchInfo> result = new List<MatchInfo>(this._cachedMatches);
            return Task.FromResult(result);
        }
        /// <summary>
        /// Asynchronously adds the specified <paramref name="match" />.
        /// </summary>
        /// <param name="match">The match.</param>
        public Task AddMatchAsync(MatchInfo match)
        {
            Guard.AgainstNullArgument("match", match);

            this.Logger.DebugFormat("Adding a new match: {0}", match);

            if (match.Id != null)
            {
                this.Logger.DebugFormat("Tried to store a match that is already stored. Id: {0}", match.Id);
                throw new InvalidOperationException("The match already has an ID.");
            }

            match.Id = this._identityGenerator.Generate();
            this._cachedMatches.Add(match);

            this._eventAggregator.PublishOnUIThread(new MatchAddedEvent(match));
            this.Logger.DebugFormat("Stored a new match with id: {0}", match.Id);

            return Task.FromResult(new object());
        }
        #endregion

        #region Implementation of IStartable
        /// <summary>
        /// Starts this instance.
        /// </summary>
        void IStartable.Start()
        {
            this.Logger.DebugFormat("Loading all matches from the data-storage.");
            this._cachedMatches = this._dataStorage.Retrieve<List<MatchInfo>>() ?? new List<MatchInfo>();
        }
        /// <summary>
        /// Stops this instance.
        /// </summary>
        void IStartable.Stop()
        {
            this.Logger.DebugFormat("Saving all matches in the data-storage.");
            this._dataStorage.Store(this._cachedMatches);
        }
        #endregion
    }
}