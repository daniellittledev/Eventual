using System;
using System.Linq;
using System.Threading.Tasks;
using Eventual.Concurrency;
using Eventual.Domain;
using Eventual.EventStore;
using Eventual.MessageContracts;
using Eventual.EventTypes;

namespace Eventual.Implementation
{
    public class Repository<T> : IRepository<T>
        where T : class, IAggregateRoot
    {
        private readonly IEventStore eventStore;
        private readonly IConflictResolver conflictResolver;
        private readonly IAggregateHydrator<T> hydrator;
        private readonly IEventBus eventBus;
        private readonly IEventClassifier eventClassifier;

        public Repository(IEventStore eventStore, IConflictResolver conflictResolver, IAggregateHydrator<T> hydrator, IEventBus eventBus, IEventClassifier eventClassifier)
        {
            this.eventStore = eventStore;
            this.conflictResolver = conflictResolver;
            this.hydrator = hydrator;
            this.eventBus = eventBus;
            this.eventClassifier = eventClassifier;
        }

        public async Task<T> LoadAsync(Guid aggregateId)
        {
            aggregateId.RequireNotDefault(nameof(aggregateId));

            try {
                var aggregateStream = await eventStore.GetStreamAsync(aggregateId);
                return hydrator.Hydrate(aggregateStream);

            } catch (StreamNotFoundException ex) {
                throw new AggregateNotFoundException(ex, aggregateId);
            }
        }

        public async Task SaveAsync(T aggregate, params object[] domainEvents)
        {
            aggregate.RequireNotNull(nameof(aggregate));
            domainEvents.RequireNotNullOrEmpty(nameof(domainEvents));

            var tasks = domainEvents
                .Select(e => eventBus.PublishAsync(e))
                .ToArray();

            await Task.WhenAll(tasks);

            var eventsToPersist = domainEvents
                .Where(x => eventClassifier.IsPersistedEvent(x.GetType()))
                .ToArray();

            await eventStore.SaveAsync(conflictResolver, aggregate.Id, aggregate.LoadedSequence, eventsToPersist);
        }
    }
}
