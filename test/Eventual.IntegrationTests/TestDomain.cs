using Eventual.Domain;
using Eventual.MessageContracts;
using System;

namespace Eventual.IntegrationTests.TestDomain
{
    internal class DomainObject : IAggregateRoot
    {
        public DomainObject(Guid id)
        {
            Id = id;
            LoadedSequence = 0;
        }

        protected DomainObject(Guid id, int loadedSequence)
        {
            Id = id;
            LoadedSequence = loadedSequence;
        }

        public Guid Id { get; }
        public int LoadedSequence { get; }
    }

    internal class DomainObjectCreatedEvent : IPersistedDomainEvent
    {
        public string Text { get; }

        public DomainObjectCreatedEvent(string text)
        {
            Text = text;
        }
    }

    internal static class DomainObjectBehaviour
    {
        public static IDomainEvent Create(this DomainObject Account, string text)
        {
            return new DomainObjectCreatedEvent(text);
        }

        public static DomainObject Apply(this DomainObject domainObject, DomainObjectCreatedEvent @event)
        {
            return domainObject;
        }
    }

    internal class DifferentDomainObject : IAggregateRoot
    {
        public DifferentDomainObject(Guid id)
        {
            Id = id;
            LoadedSequence = 0;
        }

        protected DifferentDomainObject(Guid id, int loadedSequence)
        {
            Id = id;
            LoadedSequence = loadedSequence;
        }

        public Guid Id { get; }
        public int LoadedSequence { get; }
    }
}
