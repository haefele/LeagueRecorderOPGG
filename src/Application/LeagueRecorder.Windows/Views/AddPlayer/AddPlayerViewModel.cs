using System;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro.ReactiveUI;
using LeagueRecorder.Abstractions.Data;
using ReactiveUI;

namespace LeagueRecorder.Windows.Views.AddPlayer
{
    public class AddPlayerViewModel : ReactiveScreen
    {
        private string _username;
        private ObservableAsPropertyHelper<ReactiveObservableCollection<Region>> _regions;
        private Region _selectedRegion;

        public ReactiveCommand<ReactiveObservableCollection<Region>> LoadRegions { get; private set; }
        public ReactiveCommand<object> Create { get; private set; }
        public ReactiveCommand<object> Cancel { get; private set; }

        public string Username
        {
            get { return this._username; }
            set { this.RaiseAndSetIfChanged(ref this._username, value); }
        }
        public ReactiveObservableCollection<Region> Regions
        {
            get { return this._regions.Value; }
        }
        public Region SelectedRegion
        {
            get { return this._selectedRegion; }
            set { this.RaiseAndSetIfChanged(ref this._selectedRegion, value); }
        }

        public AddPlayerViewModel()
        {
            this.CreateCommands();
        }

        protected override async void OnInitialize()
        {
            await this.LoadRegions.ExecuteAsyncTask();
        }

        private void CreateCommands()
        {
            this.LoadRegions = ReactiveCommand.CreateAsyncTask(_ =>
            {
                var result = new ReactiveObservableCollection<Region>();
                result.AddRange(Enum.GetValues(typeof(Region)).Cast<Region>());

                return Task.FromResult(result);
            });
            this.LoadRegions.ToProperty(this, f => f.Regions, out this._regions);

            this.Create = ReactiveCommand.Create(this.WhenAny(f => f.Username, f => string.IsNullOrWhiteSpace(f.Value) == false));
            this.Create.Subscribe(_ => this.TryClose(true));

            this.Cancel = ReactiveCommand.Create();
            this.Cancel.Subscribe(_ => this.TryClose(false));
        }
    }
}