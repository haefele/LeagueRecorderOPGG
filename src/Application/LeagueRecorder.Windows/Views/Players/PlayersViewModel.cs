using System;
using System.Linq;
using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.Storage;
using LeagueRecorder.Windows.Views.AddPlayer;
using LeagueRecorder.Windows.Views.Shell;
using LiteGuard;
using ReactiveUI;

namespace LeagueRecorder.Windows.Views.Players
{
    public class PlayersViewModel : ReactiveScreen, IShellTabItem
    {
        #region Fields
        private readonly IPlayerStorage _playerStorage;
        private readonly IWindowManager _windowManager;
        private readonly Func<AddPlayerViewModel> _addPlayerViewModelFactory;

        private ObservableAsPropertyHelper<ReactiveObservableCollection<Player>> _players;
        private Player _selectedPlayer;
        #endregion

        #region Properties
        /// <summary>
        /// Loads the players.
        /// </summary>
        public ReactiveCommand<ReactiveObservableCollection<Player>> LoadPlayers { get; private set; }
        /// <summary>
        /// Creates a new player.
        /// </summary>
        public ReactiveCommand<object> NewPlayer { get; private set; }
        /// <summary>
        /// Deletes the <see cref="SelectedPlayer"/>.
        /// </summary>
        public ReactiveCommand<object> DeletePlayer { get; private set; }

        /// <summary>
        /// Gets the players.
        /// </summary>
        public ReactiveObservableCollection<Player> Players
        {
            get { return this._players.Value; }
        }
        /// <summary>
        /// Gets or sets the selected player.
        /// </summary>
        public Player SelectedPlayer
        {
            get { return this._selectedPlayer; }
            set { this.RaiseAndSetIfChanged(ref this._selectedPlayer, value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersViewModel"/> class.
        /// </summary>
        /// <param name="playerStorage">The player storage.</param>
        /// <param name="windowManager">The window manager.</param>
        /// <param name="addPlayerViewModelFactory">The factory for the <see cref="AddPlayerViewModel"/>.</param>
        public PlayersViewModel(IPlayerStorage playerStorage, IWindowManager windowManager, Func<AddPlayerViewModel> addPlayerViewModelFactory)
        {
            Guard.AgainstNullArgument("playerStorage", playerStorage);
            Guard.AgainstNullArgument("windowManager", windowManager);
            Guard.AgainstNullArgument("addPlayerViewModelFactory", addPlayerViewModelFactory);

            this._playerStorage = playerStorage;
            this._windowManager = windowManager;
            this._addPlayerViewModelFactory = addPlayerViewModelFactory;

            this.CreateCommands();

            this.DisplayName = "Players";
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Called when [initialize].
        /// </summary>
        protected override async void OnInitialize()
        {
            await this.LoadPlayers.ExecuteAsyncTask();
        }
        /// <summary>
        /// Creates the commands.
        /// </summary>
        private  void CreateCommands()
        {
            this.LoadPlayers = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var players = await this._playerStorage.GetPlayersAsync();

                var result = new ReactiveObservableCollection<Player>();
                result.AddRange(players);
                return result;
            });
            this.LoadPlayers.ToProperty(this, f => f.Players, out this._players);
            this.LoadPlayers.Subscribe(_ =>
            {
                if (this.Players != null)
                    this.SelectedPlayer = this.Players.FirstOrDefault();
            });

            this.NewPlayer = ReactiveCommand.Create();
            this.NewPlayer.Subscribe(async _ =>
            {
                AddPlayerViewModel createPlayerViewModel = this._addPlayerViewModelFactory();

                if (this._windowManager.ShowDialog(createPlayerViewModel) == true)
                {
                    var player = new Player
                    {
                        Region = createPlayerViewModel.SelectedRegion,
                        Username = createPlayerViewModel.Username
                    };

                    await this._playerStorage.AddPlayerAsync(player);
                    await this.LoadPlayers.ExecuteAsyncTask();
                }
            });

            this.DeletePlayer = ReactiveCommand.Create(this.WhenAny(f => f.SelectedPlayer, f => f.Value != null));
            this.DeletePlayer.Subscribe(async _ =>
            {
                await this._playerStorage.DeletePlayerAsync(this.SelectedPlayer);
                await this.LoadPlayers.ExecuteAsyncTask();
            });
        }
        #endregion
    }
}