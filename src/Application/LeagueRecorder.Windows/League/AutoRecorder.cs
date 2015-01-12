using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
using System.Timers;
using Castle.Core.Logging;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.League;
using LeagueRecorder.Abstractions.Storage;
using LiteGuard;
using Xemio.CommonLibrary.Common;

namespace LeagueRecorder.Windows.League
{
    public class AutoRecorder : IAutoRecorder
    {
        #region Fields
        private readonly IRecordingService _recordingService;
        private readonly IMatchStorage _matchStorage;

        private Timer _recordingTimer;
        private Player[] _players;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRecorder"/> class.
        /// </summary>
        /// <param name="recordingService">The recording service.</param>
        /// <param name="matchStorage">The match storage.</param>
        public AutoRecorder(IRecordingService recordingService, IMatchStorage matchStorage)
        {
            Guard.AgainstNullArgument("recordingService", recordingService);
            Guard.AgainstNullArgument("matchStorage", matchStorage);

            this.Logger = NullLogger.Instance;

            this._recordingService = recordingService;
            this._matchStorage = matchStorage;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts to record all matches of the specified <paramref name="players" />.
        /// </summary>
        /// <param name="players">The players.</param>
        public IDisposable StartRecordingPlayerMatches(Player[] players)
        {
            if (this._recordingTimer != null)
                throw new InvalidOperationException("Recording was already started. You need to dispose the result of this method before you can start a new recording.");
            
            this.Logger.DebugFormat("Auto-recording matches of players: {0}", string.Join("|", players.Select(f => f.ToString())));

            this._players = players;

            this._recordingTimer = new Timer();
            this._recordingTimer.Interval = TimeSpan.FromMinutes(2).TotalMilliseconds;
            this._recordingTimer.Elapsed += this.CheckForMatchesToRecord;
            this._recordingTimer.Start();

            return new ActionDisposer(() =>
            {
                this._recordingTimer.Stop();
                this._recordingTimer.Elapsed -= this.CheckForMatchesToRecord;

                this._recordingTimer = null;
            });
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Checks for matches to record.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private async void CheckForMatchesToRecord(object sender, ElapsedEventArgs e)
        {
            foreach (Player player in this._players)
            {
                this.Logger.DebugFormat("Auto-recording player: {0}", player);

                MatchInfo currentMatch = await this._recordingService.GetCurrentMatchInfoFromPlayerAsync(player);

                if (currentMatch != null)
                {
                    this.Logger.DebugFormat("Found match '{0}' of player '{1}'.", currentMatch, player);
                    this.Logger.DebugFormat("Looking if it already exists.");

                    IEnumerable<MatchInfo> existingMatches = await this._matchStorage.GetMatchesAsync();

                    if (existingMatches.Any(f => f.GameId == currentMatch.GameId) == false)
                    { 
                        this.Logger.DebugFormat("The match '{0}' does not already exist.", currentMatch);
                        this.Logger.DebugFormat("Trying to record it.");

                        bool recordingStarted = await this._recordingService.RequestRecordingOfMatchAsync(currentMatch);
                        if (recordingStarted)
                        {
                            this.Logger.DebugFormat("Recording match '{0}'.", currentMatch);
                        
                            await this._matchStorage.AddMatchAsync(currentMatch);
                        }
                    }
                }
            }
        }
        #endregion
    }
}