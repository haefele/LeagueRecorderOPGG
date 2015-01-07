using System;
using System.IO;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LeagueRecorder.Abstractions.Storage;
using LeagueRecorder.Windows.Storage;
using Xemio.CommonLibrary.Storage;
using Xemio.CommonLibrary.Storage.Files;

namespace LeagueRecorder.Windows.Windsor
{
    public class StorageInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IIdentityGenerator>().ImplementedBy<IdentityGenerator>().LifestyleSingleton(),
                Component.For<IDataStorage>().Instance(this.CreateDataStorage()).LifestyleSingleton(),
                Component.For<IMatchStorage>().Forward<IStartable>().ImplementedBy<MatchStorage>().LifestyleSingleton(),
                Component.For<IPlayerStorage>().Forward<IStartable>().ImplementedBy<PlayerStorage>().LifestyleSingleton());
        }

        #region Private Methods
        /// <summary>
        /// Creates the data storage.
        /// </summary>
        private IDataStorage CreateDataStorage()
        {
            string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LeagueRecorder");

            return new DataStorage(new DataStorageSettings
            {
                FileSystem = new FileSystem(dataPath)
            });
        }
        #endregion
    }
}