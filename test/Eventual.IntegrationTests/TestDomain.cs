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

    internal class DomainObjectCreatedEvent : IDomainEvent
    {
        public string Text { get; }

        public DomainObjectCreatedEvent(string text)
        {
            Text = text;
        }
    }

    internal class DomainObjectUpdatedEvent : IDomainEvent {}

    internal static class CreateDomainObjectBehaviour
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

    internal static class UpdateDomainObjectBehaviour
    {
        public static IDomainEvent Update(this DomainObject Account)
        {
            return new DomainObjectUpdatedEvent();
        }

        public static DomainObject Apply(this DomainObject domainObject, DomainObjectUpdatedEvent @event)
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
