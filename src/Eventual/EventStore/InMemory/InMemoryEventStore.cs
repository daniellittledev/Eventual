using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Eventual.EventStore.InMemory
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly Dictionary<Guid, List<object>> eventStreams = new Dictionary<Guid, List<object>>();

        public Task<AggregateStream> GetStreamAsync(Guid streamId)
        {
            List<object> events = null;
            if (eventStreams.TryGetValue(streamId, out events)) {
                return Task.FromResult(new AggregateStream(streamId, events.Count, events, null));
            }

            throw new StreamNotFoundException(streamId);
        }

        public Task<IEnumerable<ConcurrencyEventInfo>> GetEventsAfterSequenceAsync(Guid streamId, int loadedSequence)
        {
            List<object> eventStream;
            if (eventStreams.TryGetValue(streamId, out eventStream))
            {
                return Task.FromResult(eventStream.Skip(loadedSequence)
                    .Select((x, i) => new ConcurrencyEventInfo()
                    {
                        TypeName = x.GetType().FullName,
                        Sequence = (loadedSequence + 1)
                    }));
            }

            throw new StreamNotFoundException(streamId);
        }

        public Task<StreamAppendAttempt> TryAppendToStreamAsync(Guid streamId, int loadedSequence,
            IReadOnlyCollection<object> domainEvents)
        {
            if (loadedSequence == 0)
            {
                eventStreams.Add(streamId, new List<object>());
            }
            var events = eventStreams[streamId].ToList();
            var potentialConflicts = events.Count - loadedSequence;

            if (potentialConflicts > 0)
            {
                return Task.FromResult(StreamAppendAttempt.Failure(events.Count));
            }

            eventStreams[streamId].AddRange(domainEvents);

            return Task.FromResult(StreamAppendAttempt.Success());
        }

        public Task LockStreamAsync(Guid streamId)
        {
            return Task.CompletedTask;
        }
    }
}