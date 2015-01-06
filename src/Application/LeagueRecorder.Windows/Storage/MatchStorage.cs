using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Documents;
using Castle.Core;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.Storage;
using LiteGuard;
using Xemio.CommonLibrary.Storage;

namespace LeagueRecorder.Windows.Storage
{
    public class MatchStorage : IMatchStorage, IStartable
    {
        #region Fields
        private readonly IDataStorage _dataStorage;
        private readonly IIdentityGenerator _identityGenerator;

        private List<MatchInfo> _cachedMatches; 
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchStorage"/> class.
        /// </summary>
        /// <param name="dataStorage">The data storage.</param>
        /// <param name="identityGenerator">The identity generator.</param>
        public MatchStorage(IDataStorage dataStorage, IIdentityGenerator identityGenerator)
        {
            Guard.AgainstNullArgument("dataStorage", dataStorage);
            Guard.AgainstNullArgument("identityGenerator", identityGenerator);

            this._dataStorage = dataStorage;
            this._identityGenerator = identityGenerator;
        }
        #endregion

        #region Implementation of IMatchStorage
        /// <summary>
        /// Asynchronously returns all <see cref="MatchInfo" />s.
        /// </summary>
        public Task<IEnumerable<MatchInfo>> GetMatchesAsync()
        {
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

            if (match.Id != null)
                throw new InvalidOperationException("The match already has an ID.");

            match.Id = this._identityGenerator.Generate();
            this._cachedMatches.Add(match);

            return Task.FromResult(new object());
        }
        #endregion

        #region Implementation of IStartable
        /// <summary>
        /// Starts this instance.
        /// </summary>
        void IStartable.Start()
        {
            this._cachedMatches = this._dataStorage.Retrieve<List<MatchInfo>>() ?? new List<MatchInfo>();
        }
        /// <summary>
        /// Stops this instance.
        /// </summary>
        void IStartable.Stop()
        {
            this._dataStorage.Store(this._cachedMatches);
        }
        #endregion
    }
}