using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LeagueRecorder.Abstractions;
using LeagueRecorder.Abstractions.League;
using LeagueRecorder.Windows.League;

namespace LeagueRecorder.Windows.Windsor
{
    public class ServiceInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IRecordingService>().ImplementedBy<RecordingService>().LifestyleSingleton(),
                Component.For<ISpectatorService>().ImplementedBy<SpectatorService>().LifestyleSingleton(),
                Component.For<IPlayerService>().ImplementedBy<PlayerService>().LifestyleSingleton());
        }
    }
}