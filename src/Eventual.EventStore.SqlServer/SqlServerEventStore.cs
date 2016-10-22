using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Eventual.EventStore.SqlServer
{
    public class SqlServerEventStore : IEventStore
    {
        private readonly DbConnection connection;

        public SqlServerEventStore(DbConnection connection)
        {
            this.connection = connection;
        }

        public Task<AggregateStream> GetStreamAsync(Guid streamId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConcurrencyEventInfo>> GetEventsAfterSequenceAsync(Guid streamId, int loadedSequence)
        {
            throw new NotImplementedException();
        }

        public Task<StreamAppendAttempt> TryAppendToStreamAsync(Guid streamId, int loadedSequence, IReadOnlyCollection<object> domainEvents)
        {
            throw new NotImplementedException();
            //return StreamAppendAttempt.Failure(loadedSequence + 1);
        }

        public Task LockStreamAsync(Guid streamId)
        {
            throw new NotImplementedException();
        }
    }
}
