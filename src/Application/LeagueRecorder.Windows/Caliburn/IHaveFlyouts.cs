using Caliburn.Micro.ReactiveUI;

namespace LeagueRecorder.Windows.Caliburn
{
    public interface IHaveFlyouts
    {
        ReactiveObservableCollection<FlyoutReactiveScreen> Flyouts { get; set; }  
    }
}