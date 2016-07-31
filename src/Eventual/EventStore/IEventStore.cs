using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventual.MessageContracts;

namespace Eventual.EventStore
{
    public interface IEventStore
    {
        Task<AggregateStream> GetStreamAsync(Guid streamId);
        Task SaveAsync(Guid streamId, int loadedSequence, IDomainEvent[] domainEvents);
    }

    public class AggregateStream
    {
        public Guid AggregateId { get; }
        public int LatestSequence { get; }
        public IEnumerable<IPersistedDomainEvent> Events { get; }
        public object Snapshot { get; }

        public AggregateStream(Guid aggregateId, int latestSequence, IEnumerable<IPersistedDomainEvent> events, object snapshot)
        {
            this.AggregateId = aggregateId;
            this.LatestSequence = latestSequence;
            this.Events = events;
            this.Snapshot = snapshot;
        }
    }
}