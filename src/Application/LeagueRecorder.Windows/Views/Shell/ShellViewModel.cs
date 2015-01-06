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
using LeagueRecorder.Windows.Views.Players;
using LiteGuard;
using MahApps.Metro.Controls;
using ReactiveUI;

namespace LeagueRecorder.Windows.Views.Shell
{
    public class ShellViewModel : ReactiveConductor<IShellTabItem>.Collection.OneActive
    {
        public ShellViewModel()
        {
            this.DisplayName = "League Recorder";
        }
    }
}