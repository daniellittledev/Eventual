using Eventual.Domain;
using Eventual.EventStore;
using Eventual.EventStore.Implementation.InMemory;
using Eventual.Implementation;
using System.Threading.Tasks;

namespace Eventual.IntegrationTests
{
    public static class RepositoryHelper
    {
        public static IEventApplicator GetEventApplicator()
        {
            // Singleton
            var types = TypeDiscoveryService.DiscoverTypes();
            var eventApplicator = new EventApplicator(types.ApplyExtensionMethods);
            return eventApplicator;
        }

        public static IRepository<T> BuildRepositoryWithInMemoryEventStore<T>()
            where T : class, IAggregateRoot
        {
            // Per Scope
            var eventStore = new InMemoryEventStore();

            var repository = BuildRepository< T>(eventStore);

            return repository;
        }

        public static IRepository<T> BuildRepository<T>(IEventStore eventStore)
            where T : class, IAggregateRoot
        {
            var eventApplicator = GetEventApplicator();

            // Per Scope
            var aggregateHydrator = new AggregateHydrator<T>(eventApplicator);
            var eventBus = new EventBus(_ => Task.CompletedTask);

            var repository = new Repository<T>(eventStore, aggregateHydrator, eventBus);

            return repository;
        }
    }
}
