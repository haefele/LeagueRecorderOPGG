using System;
using Caliburn.Micro.ReactiveUI;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.Storage;
using LiteGuard;
using ReactiveUI;

namespace LeagueRecorder.Windows.Views.Players
{
    public class PlayersViewModel : ReactiveScreen
    {
        #region Fields
        private readonly IPlayerStorage _playerStorage;

        private ObservableAsPropertyHelper<ReactiveObservableCollection<Player>> _players;
        private Player _selectedPlayer;
        #endregion

        #region Properties
        public ReactiveCommand<ReactiveObservableCollection<Player>> LoadPlayers { get; private set; }
        public ReactiveCommand<object> NewPlayer { get; private set; }
        public ReactiveCommand<object> DeletePlayer { get; private set; }

        public ReactiveObservableCollection<Player> Players
        {
            get { return this._players.Value; }
        }
        public Player SelectedPlayer
        {
            get { return this._selectedPlayer; }
            set { this.RaiseAndSetIfChanged(ref this._selectedPlayer, value); }
        }
        #endregion

        #region Constructors
        public PlayersViewModel(IPlayerStorage playerStorage)
        {
            Guard.AgainstNullArgument("PlayerStorage", playerStorage);

            this._playerStorage = playerStorage;

            this.CreateCommands();
        }
        #endregion

        #region Private Methods
        protected override async void OnInitialize()
        {
            await this.LoadPlayers.ExecuteAsyncTask();
        }

        private  void CreateCommands()
        {
            this.LoadPlayers = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                await this._playerStorage.AddPlayerAsync(new Player
                {
                    Region = Region.EuropeWest,
                    Username = "haefele"
                });

                var players = await this._playerStorage.GetPlayersAsync();

                var result = new ReactiveObservableCollection<Player>();
                result.AddRange(players);
                return result;
            });
            this.LoadPlayers.ToProperty(this, f => f.Players, out this._players);

            this.NewPlayer = ReactiveCommand.Create();
            this.NewPlayer.Subscribe(_ =>
            {
                var newUser = new Player();
                this.Players.Add(newUser);

                this.SelectedPlayer = newUser;
            });

            this.DeletePlayer = ReactiveCommand.Create(this.WhenAny(f => f.SelectedPlayer, f => f.Value != null));
            this.DeletePlayer.Subscribe(async _ =>
            {
                await this._playerStorage.DeletePlayerAsync(this.SelectedPlayer);
                this.Players.Remove(this.SelectedPlayer);
            });
        }
        #endregion
    }
}