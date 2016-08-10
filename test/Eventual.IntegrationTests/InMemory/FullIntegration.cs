using Eventual.IntegrationTests.TestDomain;
using System;
using Xunit;
using FluentAssertions;

namespace Eventual.IntegrationTests.InMemory
{
    public class FullIntegration
    {
        [Fact]
        public void LoadAggregateThatDoesNotExist()
        {
            var repository = RepositoryHelper.BuildRepositoryWithInMemoryEventStore<DomainObject>();

            repository
                .Invoking(x => x.LoadAsync(Guid.NewGuid()).Wait())
                .ShouldThrow<AggregateNotFoundException>();
        }
    }
}
