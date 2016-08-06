using Eventual.Domain;
using Eventual.Implementation;
using Eventual.MessageContracts;
using System;
using Xunit;
using FluentAssertions;

namespace Eventual.UnitTests.TypeDiscoveryServiceTests
{
    #region TestDomain

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

    #endregion

    public class DiscoverTypesTests
    {
        [Fact]
        public void CanFindApplyExtensionMethods()
        {
            var results = TypeDiscoveryService.DiscoverTypes(EventualUnitTestsAssembly.Assembly);
            results.ApplyExtensionMethods.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CanFindPersistedDomainEvents()
        {
            var results = TypeDiscoveryService.DiscoverTypes(EventualUnitTestsAssembly.Assembly);
            results.PersistedDomainEventTypes.Count.Should().BeGreaterThan(0);
        }
    }
}
