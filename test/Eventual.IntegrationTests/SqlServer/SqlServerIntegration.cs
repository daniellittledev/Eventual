using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Eventual.Concurrency;
using Eventual.EventStore;
using Eventual.EventStore.SqlServer;
using Eventual.Implementation;
using Eventual.IntegrationTests.TestDomain;
using FluentAssertions;
using Xunit;

namespace Eventual.IntegrationTests.SqlServer
{
    public class SqlServerIntegration
    {
        public string ConnectionString => "";

        [Fact]
        public void LoadAggregateThatDoesNotExistShouldThrow()
        {
            using (var session = new Session(ConnectionString))
            {
                session.SetupDatabase();

                var repository = new RepositoryBuilder()
                    .SetEventStore(new SqlServerEventStore(session.Connection, session.Transaction))
                    .Build<DomainObject>();

                repository
                    .Invoking(x => x.LoadAsync(Guid.NewGuid()).Wait())
                    .ShouldThrow<AggregateNotFoundException>();
            }
        }

        [Fact]
        public async Task SavingNewAggregateShouldRaiseEvents()
        {
            using (var session = new Session(ConnectionString))
            {
                session.SetupDatabase();

                var subject = new ReplaySubject<object>();

                var repository = new RepositoryBuilder()
                    .SetEventStore(new SqlServerEventStore(session.Connection, session.Transaction))
                    .SetEventBus(new EventBus(e =>
                    {
                        subject.OnNext(e);
                        return Task.CompletedTask;
                    }))
                    .Build<DomainObject>();

                var stream = subject.AsObservable();
                var domainObject = new DomainObject(Guids.Guid1);

                var events = domainObject.Create();

                await repository.SaveAsync(domainObject, events);
                subject.OnCompleted();

                (await stream).Should().BeOfType<DomainObjectCreatedEvent>();
            }
        }

        [Fact]
        public async Task SavedAggregateShouldBeAbleToBeLoaded()
        {
            using (var session = new Session(ConnectionString))
            {
                session.SetupDatabase();

                var repository = new RepositoryBuilder(EventualIntegrationTestsAssembly.Assembly)
                    .SetEventStore(new SqlServerEventStore(session.Connection, session.Transaction))
                    .Build<DomainObject>();
                var domainObject = new DomainObject(Guids.Guid1);

                var events = domainObject.Create();

                await repository.SaveAsync(domainObject, events);

                var loadedDomainObject = await repository.LoadAsync(Guids.Guid1);

                loadedDomainObject.Id.Should().Be(Guids.Guid1);
                loadedDomainObject.LoadedSequence.Should().Be(1);
            }
        }

        [Fact]
        public async Task LoadingAggregateBackAsADifferentType()
        {
            using (var session = new Session(ConnectionString))
            {
                session.SetupDatabase();

                var store = new SqlServerEventStore(session.Connection, session.Transaction);

                var repository1 = new RepositoryBuilder(EventualIntegrationTestsAssembly.Assembly)
                    .SetEventStore(store)
                    .Build<DomainObject>();
                var repository2 = new RepositoryBuilder(EventualIntegrationTestsAssembly.Assembly)
                    .SetEventStore(store)
                    .Build<DifferentDomainObject>();

                var domainObject = new DomainObject(Guids.Guid1);
                var events = domainObject.Create();

                await repository1.SaveAsync(domainObject, events);

                repository2
                    .Invoking(x => x.LoadAsync(Guids.Guid1).Wait())
                    .ShouldThrow<EventApplyAggregateMismatchException>();
            }
        }

        [Fact]
        public void SavingNewAggregateThatHasntChangedShouldThrow()
        {
            using (var session = new Session(ConnectionString))
            {
                session.SetupDatabase();

                var repository = new RepositoryBuilder()
                    .SetEventStore(new SqlServerEventStore(session.Connection, session.Transaction))
                    .Build<DomainObject>();

                var domainObject = new DomainObject(Guids.Guid1);

                repository
                    .Invoking(x => x.SaveAsync(domainObject).Wait())
                    .ShouldThrow<ArgumentException>();
            }
        }

        [Fact]
        public async Task ConcurrentSavesShouldThrow()
        {
            using (var session = new Session(ConnectionString))
            {
                session.SetupDatabase();

                var repository = new RepositoryBuilder(EventualIntegrationTestsAssembly.Assembly)
                    .SetEventStore(new SqlServerEventStore(session.Connection, session.Transaction))
                    .Build<DomainObject>();

                var domainObject = new DomainObject(Guids.Guid1);
                var events = domainObject.Create();
                await repository.SaveAsync(domainObject, events);

                var domainObject1 = await repository.LoadAsync(Guids.Guid1);
                var domainObject2 = await repository.LoadAsync(Guids.Guid1);

                var events1 = domainObject1.Update();
                await repository.SaveAsync(domainObject1, events1);

                var events2 = domainObject.Update();

                repository
                    .Invoking(x => x.SaveAsync(domainObject2, events2).Wait())
                    .ShouldThrow<EventStoreConcurrencyException>()
                    .Which.Difference.Should().Be(1);
            }
        }

        [Fact]
        public async Task ConcurrentSafeSavesShouldNotThrow()
        {
            using (var session = new Session(ConnectionString))
            {
                session.SetupDatabase();

                var repository = new RepositoryBuilder(EventualIntegrationTestsAssembly.Assembly)
                    .SetEventStore(new SqlServerEventStore(session.Connection, session.Transaction))
                    .SetConflictResolver(new ConflictResolver()
                        .RegisterNoConflict(typeof(DomainObjectSafelyUpdatedEvent), typeof(DomainObjectSafelyUpdatedEvent)))
                    .Build<DomainObject>();

                var domainObject = new DomainObject(Guids.Guid1);
                var events = domainObject.Create();
                await repository.SaveAsync(domainObject, events);

                var domainObject1 = await repository.LoadAsync(Guids.Guid1);
                var domainObject2 = await repository.LoadAsync(Guids.Guid1);

                var events1 = domainObject1.ConcurrencySafeUpdate();
                await repository.SaveAsync(domainObject1, events1);

                var events2 = domainObject.ConcurrencySafeUpdate();
                await repository.SaveAsync(domainObject2, events2);
            }
        }
    }
}
