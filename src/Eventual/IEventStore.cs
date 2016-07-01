using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventual.MessageContracts;

namespace Eventual
{
    public interface IEventStore
    {
        Task<IEnumerable<object>> GetEventsForStreamAsync(Guid streamId);
        Task SaveAsync(Guid streamId, int loadedSequence, IDomainEvent[] domainEvents);
    }
}