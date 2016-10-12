using System;
using System.Threading.Tasks;
using Eventual.MessageContracts;
using System.Collections.Generic;
using System.Linq;

namespace Eventual.EventStore.Implementation.InMemory
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly Dictionary<Guid, List<IPersistedDomainEvent>> eventStreams = new Dictionary<Guid, List<IPersistedDomainEvent>>();

        public Task<AggregateStream> GetStreamAsync(Guid streamId)
        {
            List<IPersistedDomainEvent> events = null;
            if (eventStreams.TryGetValue(streamId, out events)) {
                return Task.FromResult(new AggregateStream(streamId, events.Count, events, null));
            }

            throw new StreamNotFoundException(streamId);
        }

        public Task SaveAsync(Guid streamId, int loadedSequence, IPersistedDomainEvent[] domainEvents)
        {
            List<IPersistedDomainEvent> events = null;
            if (loadedSequence == 0) {
                events = new List<IPersistedDomainEvent>();
                eventStreams.Add(streamId, events);
            } else {
                events = eventStreams[streamId];
            }

            if (loadedSequence != events.Count) {
                throw new EventStoreConcurrencyException(streamId, loadedSequence, events.Count, events.Skip(loadedSequence).ToArray());
            }

            events.AddRange(domainEvents);

            return Task.CompletedTask;
        }
    }
}
