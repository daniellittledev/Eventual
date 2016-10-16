using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Eventual.Concurrency;

namespace Eventual.EventStore.Implementation.InMemory
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly Dictionary<Guid, List<object>> eventStreams = new Dictionary<Guid, List<object>>();

        public Task<AggregateStream> GetStreamAsync(Guid streamId)
        {
            //TODO: IEventTypeContainer

            List<object> events = null;
            if (eventStreams.TryGetValue(streamId, out events)) {
                return Task.FromResult(new AggregateStream(streamId, events.Count, events, null));
            }

            throw new StreamNotFoundException(streamId);
        }

        public Task SaveAsync(IConflictResolver conflictResolver, Guid streamId, int loadedSequence, IReadOnlyCollection<object> domainEvents)
        {
            return UpdateStream(conflictResolver, streamId, loadedSequence, domainEvents, 5);
        }

        private Task UpdateStream(IConflictResolver conflictResolver, Guid streamId, int loadedSequence, IReadOnlyCollection<object> domainEvents, int retries)
        {
            if (loadedSequence == 0) {
                eventStreams.Add(streamId, new List<object>());
            }
            var events = eventStreams[streamId].ToList();
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

                return UpdateStream(conflictResolver, streamId, (events.Count - 1), domainEvents, (retries - 1));
            }

            eventStreams[streamId].AddRange(domainEvents);
            return Task.CompletedTask;
        }
    }
}