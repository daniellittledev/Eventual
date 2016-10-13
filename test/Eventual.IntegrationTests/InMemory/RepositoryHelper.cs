using Eventual.Domain;
using Eventual.EventStore;
using Eventual.EventStore.Implementation.InMemory;
using Eventual.Implementation;
using Eventual.MessageContracts;
using System;
using System.Threading.Tasks;

namespace Eventual.IntegrationTests
{
    public static class TypesHelper
    {
        public static DiscoveredTypes DiscoveredTypes { get; }

        static TypesHelper()
        {
            DiscoveredTypes = TypeDiscoveryService.DiscoverTypes(EventualIntegrationTestsAssembly.Assembly);
        }
    }

    public static class RepositoryHelper
    {
        public static IRepository<T> BuildRepositoryWithInMemoryEventStore<T>(DiscoveredTypes types)
            where T : class, IAggregateRoot
        {
            var eventStore = new InMemoryEventStore();
            var repository = BuildRepository<T>(types, eventStore, _ => Task.CompletedTask);
            return repository;
        }

        public static IRepository<T> BuildRepositoryWithInMemoryEventStore<T>(DiscoveredTypes types, Action<IDomainEvent> onEvent)
            where T : class, IAggregateRoot
        {
            return BuildRepositoryWithInMemoryEventStore<T>(types, x => { onEvent(x); return Task.CompletedTask; });
        }

        public static IRepository<T> BuildRepositoryWithInMemoryEventStore<T>(DiscoveredTypes types, Func<IDomainEvent, Task> onEvent)
            where T : class, IAggregateRoot
        {
            var eventStore = new InMemoryEventStore();
            var repository = BuildRepository<T>(types, eventStore, onEvent);
            return repository;
        }

        public static IRepository<T> BuildRepository<T>(DiscoveredTypes types, IEventStore eventStore, Func<IDomainEvent, Task> onEvent)
            where T : class, IAggregateRoot
        {
            var eventApplicator = GetEventApplicator(types);
            var aggregateHydrator = new AggregateHydrator<T>(eventApplicator);
            var eventBus = new EventBus(onEvent);
            var repository = new Repository<T>(eventStore, aggregateHydrator, eventBus);
            return repository;
        }

        public static IEventApplicator GetEventApplicator(DiscoveredTypes types)
        {
            var eventApplicator = new EventApplicator(types.ApplyExtensionMethods);
            return eventApplicator;
        }
    }
}
