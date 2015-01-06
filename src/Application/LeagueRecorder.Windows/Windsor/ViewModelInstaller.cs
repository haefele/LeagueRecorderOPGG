﻿using Caliburn.Micro.ReactiveUI;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LeagueRecorder.Windows.Views.AddPlayer;
using LeagueRecorder.Windows.Views.Players;
using LeagueRecorder.Windows.Views.Shell;

namespace LeagueRecorder.Windows.Windsor
{
    public class ViewModelInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ShellViewModel>().LifestyleTransient(),
                Component.For<PlayersViewModel>().Forward<IShellTabItem>().LifestyleTransient(),
                Component.For<AddPlayerViewModel>().LifestyleTransient());
        }
    }
}