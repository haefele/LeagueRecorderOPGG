using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Timers;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.League;
using LeagueRecorder.Abstractions.Storage;
using Xemio.CommonLibrary.Common;

namespace LeagueRecorder.Windows.League
{
    public class AutoRecorder : IAutoRecorder
    {
        private readonly IRecordingService _recordingService;
        private readonly IMatchStorage _matchStorage;

        private Timer _recordingTimer;
        private Player[] _players;

        public AutoRecorder(IRecordingService recordingService, IMatchStorage matchStorage)
        {
            _recordingService = recordingService;
            _matchStorage = matchStorage;
        }

        public IDisposable StartRecordingPlayerMatches(Player[] players)
        {
            if (this._recordingTimer != null)
                throw new InvalidOperationException("Recording was already started. You need to dispose the result of this method before you can start a new recording.");

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

        private async void CheckForMatchesToRecord(object sender, ElapsedEventArgs e)
        {
            foreach (Player player in this._players)
            {
                MatchInfo currentMatch = await this._recordingService.GetCurrentMatchInfoFromPlayerAsync(player);

                if (currentMatch != null)
                {
                    bool recordingStarted = await this._recordingService.RequestRecordingOfMatchAsync(currentMatch);
                    if (recordingStarted)
                    {
                        await this._matchStorage.AddMatchAsync(currentMatch);
                    }
                }
            }
        }
    }
}