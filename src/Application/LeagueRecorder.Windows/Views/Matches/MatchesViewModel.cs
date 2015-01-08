using Caliburn.Micro.ReactiveUI;
using LeagueRecorder.Windows.Views.Shell;

namespace LeagueRecorder.Windows.Views.Matches
{
    public class MatchesViewModel : ReactiveScreen, IShellTabItem
    {
        public MatchesViewModel()
        {
            this.DisplayName = "Matches";
        }
    }
}