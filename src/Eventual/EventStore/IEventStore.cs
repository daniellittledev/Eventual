using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventual.MessageContracts;

namespace Eventual.EventStore
{
    public interface IEventStore
    {
        Task<AggregateStream> GetStreamAsync(Guid streamId);
        Task SaveAsync(Guid streamId, int loadedSequence, IPersistedDomainEvent[] domainEvents);
    }

    public class AggregateStream
    {
        public Guid StreamId { get; }
        public int LatestSequence { get; }
        public IEnumerable<IPersistedDomainEvent> Events { get; }
        public object Snapshot { get; }

        public AggregateStream(Guid streamId, int latestSequence, IEnumerable<IPersistedDomainEvent> events, object snapshot)
        {
            this.StreamId = streamId;
            this.LatestSequence = latestSequence;
            this.Events = events;
            this.Snapshot = snapshot;
        }
    }
}