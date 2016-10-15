using System;
using System.Threading.Tasks;
using Eventual.MessageContracts;
using System.Collections.Generic;
using System.Linq;
using Eventual.Concurrency;

namespace Eventual.EventStore.Implementation.InMemory
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly IConflictResolver conflictResolver;
        private readonly Dictionary<Guid, List<IPersistedDomainEvent>> eventStreams = new Dictionary<Guid, List<IPersistedDomainEvent>>();

        public InMemoryEventStore(IConflictResolver conflictResolver)
        {
            this.conflictResolver = conflictResolver;
        }

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
            return UpdateStream(streamId, loadedSequence, domainEvents, 5);
        }

        private Task UpdateStream(Guid streamId, int loadedSequence, IPersistedDomainEvent[] domainEvents, int retries)
        {
            if (loadedSequence == 0) {
                eventStreams.Add(streamId, new List<IPersistedDomainEvent>());
            }
            List<IPersistedDomainEvent> events = eventStreams[streamId].ToList();
            var potentialConflicts = events.Count - loadedSequence;

            if (potentialConflicts < 0) {
                throw new EventStoreConcurrencyException(streamId, loadedSequence, events.Count);
            }

            if (potentialConflicts > 0) {
                var newEvents = events
                    .Skip(loadedSequence)
                    .ToArray();

                var newEventTypes = events
                    .Select(x => x.GetType())
                    .ToArray();

                var concurrencyException = false;

                foreach (var domainEvent in domainEvents) {
                    if (conflictResolver.ConflictsWith(domainEvent.GetType(), newEventTypes)) {
                        concurrencyException = true;
                        break;
                    }
                }

                if (concurrencyException || retries <= 0) {
                    throw new EventStoreConcurrencyException(streamId, loadedSequence, events.Count, newEvents);
                }

                return UpdateStream(streamId, (events.Count - 1), domainEvents, (retries - 1));
            }

            eventStreams[streamId].AddRange(domainEvents);
            return Task.CompletedTask;
        }
    }
}