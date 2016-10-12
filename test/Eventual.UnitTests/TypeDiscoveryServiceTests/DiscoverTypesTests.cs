using Eventual.Implementation;
using Xunit;
using FluentAssertions;
using Eventual.EventStore;
using System;
using System.Linq;

namespace Eventual.UnitTests.TypeDiscoveryServiceTests
{
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
