using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventual.EventStore
{
    public interface IEventManager
    {
        Task<AggregateStream> GetStreamAsync(Guid streamId);


        Task SaveAsync(Guid streamId, int loadedSequence, IReadOnlyCollection<object> domainEvents);
    }
}