using Autofac;
using Eventual.EventStore.Implementation.InMemory;
using Eventual.Implementation;

namespace Eventual.Autofac
{
    public class EventualModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>)).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(AggregateHydrator<>)).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<EventApplicator>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<InMemoryEventStore>().AsImplementedInterfaces().InstancePerLifetimeScope();

        }
    }
}
