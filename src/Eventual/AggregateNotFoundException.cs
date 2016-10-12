using System;

namespace Eventual
{
    public class AggregateNotFoundException : Exception
    {
        public Guid AggregateId { get; }

        public AggregateNotFoundException(Exception ex, Guid aggregateId)
            : base(string.Empty, ex)
        {
            AggregateId = aggregateId;
        }

        public override string Message {
            get {
                return $"No aggregate with Id {AggregateId} could be found";
            }
        }
    }
}
