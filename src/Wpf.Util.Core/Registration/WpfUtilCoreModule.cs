using Autofac;
using Wpf.Util.Core.Diagnostics;
using Wpf.Util.Core.ViewModels;

namespace Wpf.Util.Core.Registration
{
    public class WpfUtilCoreModule :Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var logger = new LogViewModel(1000, 100);
            builder.RegisterInstance(logger).As<ILogger>().SingleInstance();
            builder.RegisterType<CommandTreeItemViewMapper>().As<ICommandTreeItemViewMapper>().SingleInstance();
        }
    }
}
