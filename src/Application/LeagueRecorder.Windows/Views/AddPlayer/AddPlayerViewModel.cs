using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro.ReactiveUI;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.League;
using LiteGuard;
using ReactiveUI;

namespace LeagueRecorder.Windows.Views.AddPlayer
{
    public class AddPlayerViewModel : ReactiveScreen
    {
        #region Fields
        private readonly IPlayerService _playerService;
        
        private string _username;
        private ObservableAsPropertyHelper<ReactiveObservableCollection<Region>> _regions;
        private Region _selectedRegion;
        #endregion

        #region Properties
        /// <summary>
        /// Loads all available regions.
        /// </summary>
        public ReactiveCommand<ReactiveObservableCollection<Region>> LoadRegions { get; private set; }
        /// <summary>
        /// Creates the player.
        /// </summary>
        public ReactiveCommand<Unit> Create { get; private set; }
        /// <summary>
        /// Cancels to create the player.
        /// </summary>
        public ReactiveCommand<object> Cancel { get; private set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get { return this._username; }
            set { this.RaiseAndSetIfChanged(ref this._username, value); }
        }
        /// <summary>
        /// Gets the regions.
        /// </summary>
        public ReactiveObservableCollection<Region> Regions
        {
            get { return this._regions.Value; }
        }
        /// <summary>
        /// Gets or sets the selected region.
        /// </summary>
        public Region SelectedRegion
        {
            get { return this._selectedRegion; }
            set { this.RaiseAndSetIfChanged(ref this._selectedRegion, value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AddPlayerViewModel" /> class.
        /// </summary>
        /// <param name="playerService">The player service.</param>
        public AddPlayerViewModel(IPlayerService playerService)
        {
            Guard.AgainstNullArgument("playerService", playerService);

            this._playerService = playerService;

            this.CreateCommands();

            this.DisplayName = "Add Player";
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Called when [initialize].
        /// </summary>
        protected override async void OnInitialize()
        {
            await this.LoadRegions.ExecuteAsyncTask();
        }
        /// <summary>
        /// Creates the commands.
        /// </summary>
        private void CreateCommands()
        {
            this.LoadRegions = ReactiveCommand.CreateAsyncTask(_ =>
            {
                var result = new ReactiveObservableCollection<Region>();
                result.AddRange(Enum.GetValues(typeof(Region)).Cast<Region>());

                return Task.FromResult(result);
            });
            this.LoadRegions.ToProperty(this, f => f.Regions, out this._regions);

            this.Create = ReactiveCommand.CreateAsyncTask(
                this.WhenAny(f => f.Username, f => string.IsNullOrWhiteSpace(f.Value) == false),
                async _ =>
                {
                    bool playerExists = await this._playerService.PlayerExists(this.Username, this.SelectedRegion);
                    
                    if (playerExists)
                    { 
                        this.TryClose(true);
                        return;
                    }

                    MessageBox.Show(string.Format("No player with the username {0} exists.", this.Username));
                });

            this.Cancel = ReactiveCommand.Create();
            this.Cancel.Subscribe(_ => this.TryClose(false));
        }
        #endregion
    }
}