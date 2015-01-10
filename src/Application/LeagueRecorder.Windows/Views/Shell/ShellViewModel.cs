using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.League;
using LeagueRecorder.Abstractions.Storage;
using LeagueRecorder.Windows.Caliburn;
using LeagueRecorder.Windows.Events;
using LeagueRecorder.Windows.Views.Players;
using LiteGuard;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace LeagueRecorder.Windows.Views.Shell
{
    public class ShellViewModel : ReactiveConductor<IShellTabItem>.Collection.OneActive, IHandleWithTask<PlayerEvent>
    {
        #region Fields
        private readonly IPlayerStorage _playerStorage;
        private readonly IAutoRecorder _autoRecorder;
        private readonly IEventAggregator _eventAggregator;

        private IDisposable _autoRecorderDisposable;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="tabItems">The tab items.</param>
        /// <param name="playerStorage">The player storage.</param>
        /// <param name="autoRecorder">The automatic recorder.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public ShellViewModel(IShellTabItem[] tabItems, IPlayerStorage playerStorage, IAutoRecorder autoRecorder, IEventAggregator eventAggregator)
        {
            Guard.AgainstNullArgument("tabItems", tabItems);
            Guard.AgainstNullArgument("playerStorage", playerStorage);
            Guard.AgainstNullArgument("autoRecorder", autoRecorder);
            Guard.AgainstNullArgument("eventAggregator", eventAggregator);

            this.Items.AddRange(tabItems);
            this._playerStorage = playerStorage;
            this._autoRecorder = autoRecorder;
            this._eventAggregator = eventAggregator;

            this.DisplayName = "League Recorder";

            this._eventAggregator.Subscribe(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public async Task Handle(PlayerEvent message)
        {
            await RestartAutoRecorder();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Called when [initialize].
        /// </summary>
        protected override async void OnInitialize()
        {
            await this.RestartAutoRecorder();
        }
        /// <summary>
        /// Restarts the automatic recorder.
        /// </summary>
        private async Task RestartAutoRecorder()
        {
            if (this._autoRecorderDisposable != null)
                this._autoRecorderDisposable.Dispose();

            IEnumerable<Player> players = await this._playerStorage.GetPlayersAsync();
            this._autoRecorderDisposable = this._autoRecorder.StartRecordingPlayerMatches(players.ToArray());
        }
        #endregion
    }
}