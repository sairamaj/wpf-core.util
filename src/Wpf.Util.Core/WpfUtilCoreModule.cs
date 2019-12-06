using Autofac;
using Wpf.Util.Core.Diagnostics;
using Wpf.Util.Core.ViewModels;

namespace Wpf.Util.Core
{
    public class WpfUtilCoreModule :Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var logger = new LogViewModel();
            builder.RegisterInstance(logger).As<ILogger>().SingleInstance();
        }
    }
}
