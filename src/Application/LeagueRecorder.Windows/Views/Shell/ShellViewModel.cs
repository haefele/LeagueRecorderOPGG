using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using LeagueRecorder.Abstractions.Data;
using LeagueRecorder.Abstractions.Storage;
using LeagueRecorder.Windows.Caliburn;
using LiteGuard;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace LeagueRecorder.Windows.Views.Shell
{
    public class ShellViewModel : ReactiveScreen, IHaveFlyouts
    {
        public ReactiveObservableCollection<FlyoutReactiveScreen> Flyouts { get; set; }

        public ShellViewModel()
        {
            this.Flyouts = new ReactiveObservableCollection<FlyoutReactiveScreen>();
            this.Flyouts.Add(new FlyoutReactiveScreen
            {
                DisplayName = "Hallo Welt",
                Position = Position.Right
            });
        }

        public void ShowFlyout()
        {
            this.Flyouts.First().Toggle();
        }
    }
}