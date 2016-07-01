using System;

namespace Eventual
{
    public interface IAggregateRoot
    {
        Guid AggregateId { get; }
        int LoadedSequence { get; }
    }
}
