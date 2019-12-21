using System;
using Autofac;
using Autofac.Core;

namespace Wpf.Util.Core.Registration
{
    /// <summary>
    /// Service locator.
    /// </summary>
    public class ServiceLocator : IServiceLocator
    {
        /// <summary>
        /// Container instance.
        /// </summary>
        private static IContainer _container;

        /// <summary>
        /// Initializes the container.
        /// </summary>
        /// <param name="builder">
        /// A instance of <see cref="ContainerBuilder"/>.
        /// </param>
        public void Initialize(ContainerBuilder builder)
        {
            builder.RegisterModule(new WpfUtilCoreModule());
            _container = builder.Build();
        }

        /// <summary>
        /// Resolves the object from container.
        /// </summary>
        /// <typeparam name="T">Type name.</typeparam>
        /// <returns>Resolved instance.</returns>
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        /// Resolves the object from container.
        /// </summary>
        /// <typeparam name="T">Type name.</typeparam>
        /// <param name="parameters">Resolution parameters.</param>
        /// <returns>Resolved instance.</returns>
        public T Resolve<T>(params Parameter[] parameters)
        {
            return _container.Resolve<T>(parameters);
        }

        /// <summary>
        /// Resolves the object for given type.
        /// </summary>
        /// <param name="serviceType">
        /// A <see cref="Type"/> service type.
        /// </param>
        /// <returns>
        /// A service instance.
        /// </returns>
        public object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }
    }
}
