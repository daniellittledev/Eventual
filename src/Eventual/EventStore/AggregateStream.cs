using System;
using System.Collections.Generic;

namespace Eventual.EventStore
{
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