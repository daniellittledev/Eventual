using System;
using System.Linq;
using System.Threading.Tasks;
using Eventual.Domain;
using Eventual.EventStore;
using Eventual.EventTypes;

namespace Eventual.Implementation
{
    public class Repository<T> : IRepository<T>
        where T : class, IAggregateRoot
    {
        private readonly IEventManager eventManager;
        private readonly IAggregateHydrator<T> hydrator;
        private readonly IEventBus eventBus;
        private readonly IEventClassifier eventClassifier;

        public Repository(IEventManager eventManager, IAggregateHydrator<T> hydrator, IEventBus eventBus, IEventClassifier eventClassifier)
        {
            this.eventManager = eventManager;
            this.hydrator = hydrator;
            this.eventBus = eventBus;
            this.eventClassifier = eventClassifier;
        }

        public async Task<T> LoadAsync(Guid aggregateId)
        {
            aggregateId.RequireNotDefault(nameof(aggregateId));

            try {
                var aggregateStream = await eventManager.GetStreamAsync(aggregateId);
                return hydrator.Hydrate(aggregateStream);

            } catch (StreamNotFoundException ex) {
                throw new AggregateNotFoundException(ex, aggregateId);
            }
        }

        public async Task SaveAsync(T aggregate, params object[] domainEvents)
        {
            aggregate.RequireNotNull(nameof(aggregate));
            domainEvents.RequireNotNullOrEmpty(nameof(domainEvents));

            if (domainEvents.Length == 0) {
                throw new SavedAnAggregateThatHadNoChangesException(aggregate.Id);
            }

            var tasks = domainEvents
                .Select(e => eventBus.PublishAsync(e))
                .ToArray();

            await Task.WhenAll(tasks);

            var eventsToPersist = domainEvents
                .Where(x => eventClassifier.IsPersistedEvent(x.GetType()))
                .ToArray();

            await eventManager.SaveAsync(aggregate.Id, aggregate.LoadedSequence, eventsToPersist);

        }
    }
}
