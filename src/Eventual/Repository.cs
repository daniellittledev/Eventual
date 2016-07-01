using System;
using System.Threading.Tasks;
using Eventual.MessageContracts;

namespace Eventual
{
    public class Repository<T>
        where T : IAggregateRoot
    {
        private readonly IEventStore eventStore;
        private readonly IAggregateHydrator hydrator;

        public Repository(IEventStore eventStore, IAggregateHydrator hydrator)
        {
            this.eventStore = eventStore;
            this.hydrator = hydrator;
        }

        public async Task<T> LoadAsync(Guid aggregateId)
        {
            var events = await eventStore.GetEventsForStreamAsync(aggregateId);
            return hydrator.Hydrate<T>(events);
        }

        public async void SaveAsync(T aggregate, params IDomainEvent[] domainEvents)
        {
            // Send out the domain events;

            await eventStore.SaveAsync(aggregate.AggregateId, aggregate.LoadedSequence, domainEvents);
        }
    }
}
