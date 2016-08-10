using Autofac;
using Enexure.MicroBus;
using Enexure.MicroBus.Autofac;

namespace Eventual.Examples
{
    public static class TestHelper
    {
        public static ILifetimeScope GetSystem()
        {
            var containerBuilder = new ContainerBuilder();

            var busBuilder = new BusBuilder();
            busBuilder.RegisterHandlers(x => true, EventualExamplesAssembly.Assembly);

            containerBuilder.RegisterMicroBus(busBuilder);

            var container = containerBuilder.Build();

            return container;
        }
    }
}
