using System;
using System.Linq;
using System.Threading.Tasks;
using Eventual.Domain;
using Eventual.EventStore;
using Eventual.MessageContracts;

namespace Eventual.Implementation
{
    public class Repository<T>
        where T : class, IAggregateRoot
    {
        private readonly IEventStore eventStore;
        private readonly IAggregateHydrator<T> hydrator;
        private readonly IEventBus eventBus;

        public Repository(IEventStore eventStore, IAggregateHydrator<T> hydrator, IEventBus eventBus)
        {
            this.eventStore = eventStore;
            this.hydrator = hydrator;
            this.eventBus = eventBus;
        }

        public async Task<T> LoadAsync(Guid aggregateId)
        {
            aggregateId.RequireNotDefault(nameof(aggregateId));

            var aggregateStream = await eventStore.GetStreamAsync(aggregateId);
            return hydrator.Hydrate(aggregateStream);
        }

        public async void SaveAsync(T aggregate, params IDomainEvent[] domainEvents)
        {
            aggregate.RequireNotNull(nameof(aggregate));
            domainEvents.RequireNotNullOrEmpty(nameof(domainEvents));

            await Task.WhenAll(domainEvents.Select(e => eventBus.PublishAsync(e)).ToArray());

            await eventStore.SaveAsync(aggregate.Id, aggregate.LoadedSequence, domainEvents);
        }
    }
}
