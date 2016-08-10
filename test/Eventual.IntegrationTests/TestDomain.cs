﻿using Eventual.Domain;
using Eventual.MessageContracts;
using System;

namespace Eventual.IntegrationTests.TestDomain
{
    internal class DomainObject : IAggregateRoot
    {
        public DomainObject(Guid id, int loadedSequence)
        {
            Id = id;
            LoadedSequence = loadedSequence;
        }

        public Guid Id { get; }
        public int LoadedSequence { get; }
    }

    internal class SampleEvent : IPersistedDomainEvent
    {
        public int EventId { get; set; }
    }

    internal static class DomainObjectBehaviour
    {
        public static DomainObject Apply(this DomainObject domainObject, SampleEvent @event)
        {
            return domainObject;
        }
    }
}
