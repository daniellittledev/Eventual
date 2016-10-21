using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventual.Concurrency;
using Eventual.MessageContracts;

namespace Eventual.EventStore
{
    public interface IEventStore
    {
        Task<AggregateStream> GetStreamAsync(Guid streamId);


        Task SaveAsync(Guid streamId, int loadedSequence, IReadOnlyCollection<object> domainEvents, IConflictResolver conflictResolver);
    }

    public class AggregateStream
    {
        public Guid StreamId { get; }
        public int LatestSequence { get; }
        public IEnumerable<object> Events { get; }
        public object Snapshot { get; }

        public AggregateStream(Guid streamId, int latestSequence, IEnumerable<object> events, object snapshot)
        {
            this.StreamId = streamId;
            this.LatestSequence = latestSequence;
            this.Events = events;
            this.Snapshot = snapshot;
        }
    }
}