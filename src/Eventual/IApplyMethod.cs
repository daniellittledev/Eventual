using Eventual.Domain;
using Eventual.MessageContracts;
using System;

namespace Eventual.Implementation
{
    public interface IApplyMethod
    {
        Type AggregateRootType { get; }
        Type EventType { get; }

        IAggregateRoot Invoke(IAggregateRoot aggregateRoot, IPersistedDomainEvent domainEvent);
    }
}