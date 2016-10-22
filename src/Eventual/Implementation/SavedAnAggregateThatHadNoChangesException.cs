using System;

namespace Eventual.Implementation
{
    public class SavedAnAggregateThatHadNoChangesException : Exception
    {
        public SavedAnAggregateThatHadNoChangesException(Guid id)
            : base(string.Format("The aggregate {0} had no changes to save or publish", id))
        {
            this.AggregateId = id;
        }

        public Guid AggregateId { get; }
    }
}