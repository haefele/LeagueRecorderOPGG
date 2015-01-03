using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Akavache;
using Castle.Core;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.Storage;
using LiteGuard;

namespace LeagueRecorder.Windows.Storage
{
    public class MatchStorage : IMatchStorage
    {
        #region Fields
        private readonly IBlobCache _blobCache;
        private readonly IIdentityGenerator _identityGenerator;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchStorage"/> class.
        /// </summary>
        /// <param name="blobCache">The BLOB cache.</param>
        /// <param name="identityGenerator">The identity generator.</param>
        public MatchStorage(IBlobCache blobCache, IIdentityGenerator identityGenerator)
        {
            Guard.AgainstNullArgument("blobCache", blobCache);
            Guard.AgainstNullArgument("identityGenerator", identityGenerator);

            this._blobCache = blobCache;
            this._identityGenerator = identityGenerator;
        }
        #endregion

        #region Implementation of IMatchStorage
        /// <summary>
        /// Asynchronously returns all <see cref="MatchInfo" />s.
        /// </summary>
        public Task<IEnumerable<MatchInfo>> GetMatchesAsync()
        {
            return this._blobCache.GetAllObjects<MatchInfo>().ToTask();
        }
        /// <summary>
        /// Asynchronously adds the specified <paramref name="match" />.
        /// </summary>
        /// <param name="match">The match.</param>
        public async Task AddMatchAsync(MatchInfo match)
        {
            Guard.AgainstNullArgument("match", match);

            if (match.Id != null)
                throw new InvalidOperationException("The match already has an ID.");

            match.Id = this._identityGenerator.Generate();
            await this._blobCache.InsertObject(match.Id, match).ToTask().ConfigureAwait(false);
        }
        #endregion
    }
}