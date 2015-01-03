using Akavache;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace LeagueRecorder.Windows.Windsor
{
    public class AkavacheInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            BlobCache.ApplicationName = "LeagueRecorderOPGG";

            container.Register(
                Component.For<IBlobCache>().Instance(BlobCache.UserAccount));
        }
    }
}