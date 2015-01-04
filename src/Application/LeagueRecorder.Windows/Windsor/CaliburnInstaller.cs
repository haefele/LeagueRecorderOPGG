using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LeagueRecorder.Windows.Caliburn;

namespace LeagueRecorder.Windows.Windsor
{
    public class CaliburnInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IWindowManager>().ImplementedBy<MetroWindowManager>().LifestyleSingleton());
        }
    }
}