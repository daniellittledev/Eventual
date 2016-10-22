using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventual.EventStore
{
    public interface IEventStore
    {
        Task<AggregateStream> GetStreamAsync(Guid streamId);

        Task<IEnumerable<ConcurrencyEventInfo>> GetEventsAfterSequenceAsync(Guid streamId, int loadedSequence);

        Task<StreamAppendAttempt> TryAppendToStreamAsync(Guid streamId, int loadedSequence, IReadOnlyCollection<object> domainEvents);
        Task LockStreamAsync(Guid streamId);
    }
}