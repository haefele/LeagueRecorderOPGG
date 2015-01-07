using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using Castle.Core.Logging;
using Castle.Windsor;
using Castle.Windsor.Installer;
using LeagueRecorder.Windows.Views.Shell;

namespace LeagueRecorder.Windows
{
    public class Bootstrapper : BootstrapperBase
    {
        #region Fields
        private IWindsorContainer _container;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
            this.Initialize();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Override to configure the framework and setup your IoC container.
        /// </summary>
        protected override void Configure()
        {
            this._container = new WindsorContainer();
            this._container.Install(FromAssembly.This());
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        protected override object GetInstance(Type service, string key)
        {
            if (this._container.Kernel.HasComponent(service) == false)
                return base.GetInstance(service, key);

            return this._container.Resolve(service);
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation
        /// </summary>
        /// <param name="service">The service to locate.</param>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            if (this._container.Kernel.HasComponent(service) == false)
                return base.GetAllInstances(service);

            return this._container.ResolveAll(service).OfType<object>();
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            IEnumerable<PropertyInfo> propertiesToInject = instance
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => f.CanWrite && this._container.Kernel.HasComponent(f.PropertyType));

            foreach (var property in propertiesToInject)
            {
                property.SetValue(instance, this._container.Resolve(property.PropertyType));
            }
        }
        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            dynamic settings = new ExpandoObject();
            settings.SizeToContent = SizeToContent.WidthAndHeight;
            settings.ResizeMode = ResizeMode.NoResize;

            this.DisplayRootViewFor<ShellViewModel>(settings);
        }
        /// <summary>
        /// Override this to add custom behavior on exit.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected override void OnExit(object sender, EventArgs e)
        {
            this._container.Dispose();
        }
        /// <summary>
        /// Override this to add custom behavior for unhandled exceptions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var loggerFactory = this._container.Resolve<ILoggerFactory>();

            ILogger logger = loggerFactory.Create(this.GetType());
            logger.Error("An unhandled exception occured.", e.Exception);
        }
        #endregion
    }
}