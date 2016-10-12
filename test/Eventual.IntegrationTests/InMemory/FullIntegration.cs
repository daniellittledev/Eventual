using Eventual.IntegrationTests.TestDomain;
using System;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using Eventual.MessageContracts;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace Eventual.IntegrationTests.InMemory
{
    public class FullIntegration
    {
        [Fact]
        public void LoadAggregateThatDoesNotExistShouldThrow()
        {
            var repository = RepositoryHelper.BuildRepositoryWithInMemoryEventStore<DomainObject>(TypesHelper.DiscoveredTypes);

            repository
                .Invoking(x => x.LoadAsync(Guid.NewGuid()).Wait())
                .ShouldThrow<AggregateNotFoundException>();
        }

        [Fact]
        public async Task SavingNewAggregateShouldRaiseEvents()
        {
            var data = "Hello";
            var subject = new ReplaySubject<IDomainEvent>();
            var repository = RepositoryHelper.BuildRepositoryWithInMemoryEventStore<DomainObject>(TypesHelper.DiscoveredTypes, e => subject.OnNext(e));
            var stream = subject.AsObservable();
            var domainObject = new DomainObject(Guids.Guid1);
            var events = domainObject.Create(data);

            await repository.SaveAsync(domainObject, events);
            subject.OnCompleted();

            (await stream).Should().BeOfType<DomainObjectCreatedEvent>().Which.Text.Should().Be(data);
        }

        [Fact]
        public async Task SavedAggregateShouldBeAbleToBeLoaded()
        {
            var data = "Hello";
            var repository = RepositoryHelper.BuildRepositoryWithInMemoryEventStore<DomainObject>(TypesHelper.DiscoveredTypes);
            var domainObject = new DomainObject(Guids.Guid1);
            var events = domainObject.Create(data);

            await repository.SaveAsync(domainObject, events);

            var loadedDomainObject = await repository.LoadAsync(Guids.Guid1);

            loadedDomainObject.Id.Should().Be(Guids.Guid1);
            loadedDomainObject.LoadedSequence.Should().Be(1);
        }

        [Fact]
        public void SavingNewAggregateThatHasntChangedShouldThrow()
        {
            var repository = RepositoryHelper.BuildRepositoryWithInMemoryEventStore<DomainObject>(TypesHelper.DiscoveredTypes);

            var domainObject = new DomainObject(Guids.Guid1);

            repository
                .Invoking(x => x.SaveAsync(domainObject).Wait())
                .ShouldThrow<ArgumentException>();
        }
    }
}
