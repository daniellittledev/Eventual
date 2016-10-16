using Eventual.IntegrationTests.TestDomain;
using System;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using Eventual.EventStore.Implementation.InMemory;
using Eventual.Implementation;

namespace Eventual.IntegrationTests.InMemory
{
    public class FullIntegration
    {
        [Fact]
        public void LoadAggregateThatDoesNotExistShouldThrow()
        {
            var repository = new RepositoryBuilder().Build<DomainObject>();

            repository
                .Invoking(x => x.LoadAsync(Guid.NewGuid()).Wait())
                .ShouldThrow<AggregateNotFoundException>();
        }

        [Fact]
        public async Task SavingNewAggregateShouldRaiseEvents()
        {
            var subject = new ReplaySubject<object>();

            var repository = new RepositoryBuilder()
                .SetEventBus(new EventBus(e => { subject.OnNext(e); return Task.CompletedTask; }))
                .Build<DomainObject>();

            var stream = subject.AsObservable();
            var domainObject = new DomainObject(Guids.Guid1);

            var data = "Hello";
            var events = domainObject.Create(data);

            await repository.SaveAsync(domainObject, events);
            subject.OnCompleted();

            (await stream).Should().BeOfType<DomainObjectCreatedEvent>().Which.Text.Should().Be(data);
        }

        [Fact]
        public async Task SavedAggregateShouldBeAbleToBeLoaded()
        {
            var repository = new RepositoryBuilder(EventualIntegrationTestsAssembly.Assembly).Build<DomainObject>();
            var domainObject = new DomainObject(Guids.Guid1);

            var data = "Hello";
            var events = domainObject.Create(data);

            await repository.SaveAsync(domainObject, events);

            var loadedDomainObject = await repository.LoadAsync(Guids.Guid1);

            loadedDomainObject.Id.Should().Be(Guids.Guid1);
            loadedDomainObject.LoadedSequence.Should().Be(1);
        }

        [Fact]
        public async Task LoadingAggregateBackAsADifferentType()
        {
            var store = new InMemoryEventStore();
            var repository1 = new RepositoryBuilder(EventualIntegrationTestsAssembly.Assembly).SetEventStore(store).Build<DomainObject>();
            var repository2 = new RepositoryBuilder(EventualIntegrationTestsAssembly.Assembly).SetEventStore(store).Build<DifferentDomainObject>();

            var domainObject = new DomainObject(Guids.Guid1);
            var events = domainObject.Create(string.Empty);

            await repository1.SaveAsync(domainObject, events);

            repository2
                .Invoking(x => x.LoadAsync(Guids.Guid1).Wait())
                .ShouldThrow<EventApplyAggregateMismatchException>();
        }

        [Fact]
        public void SavingNewAggregateThatHasntChangedShouldThrow()
        {
            var repository = new RepositoryBuilder().Build<DomainObject>();

            var domainObject = new DomainObject(Guids.Guid1);

            repository
                .Invoking(x => x.SaveAsync(domainObject).Wait())
                .ShouldThrow<ArgumentException>();
        }
    }
}
