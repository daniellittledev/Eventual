using Xunit;
using Eventual.TypeDiscovery;
using FluentAssertions;

namespace Eventual.UnitTests.TypeDiscoveryServiceTests
{
    public class DiscoverTypesTests
    {
        [Fact]
        public void CanFindApplyExtensionMethods()
        {
            var applyExtensionMethods = new EventApplyLocator();
            TypeDiscoveryService.DiscoverTypes(new IScanningLocator[] { applyExtensionMethods }, EventualUnitTestsAssembly.Assembly);
            applyExtensionMethods.ApplyMethods.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CanFindPersistedDomainEvents()
        {
            var domainEvents = new DomainEventLocator((x, i) => x.FullName.EndsWith("Event"));
            TypeDiscoveryService.DiscoverTypes(new IScanningLocator[] { domainEvents }, EventualUnitTestsAssembly.Assembly);
            domainEvents.EventTypes.Count.Should().BeGreaterThan(0);
        }
    }
}
