using System;
using System.Linq;
using Eventual.Domain;
using Eventual.Implementation;
using Eventual.EventStore;
using Eventual.MessageContracts;
using Xunit;
using FluentAssertions;
using Moq;

namespace Eventual.UnitTests
{
    public class HydrateTests
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

        [Fact]
        public void HydrateMustSetTheIdAndLoadedSequenceOfTheDomainObject()
        {
            var eventApplicator = new Mock<IEventApplicator<DomainObject>>();
            var hydrator = new AggregateHydrator<DomainObject>(eventApplicator.Object);

            var id = new Guid("{5D776F58-AB9D-4AAF-805D-602F876DFA1A}");

            var domainObject = hydrator.Hydrate(new AggregateStream(id, 1, Enumerable.Empty<IPersistedDomainEvent>(), null));

            domainObject.Id.Should().Be(id);
            domainObject.LoadedSequence.Should().Be(1);
        }

        [Fact]
        public void HydrateMustCallApplyEventOnTheApplicatorForEachEvent()
        {
            var events = new[] {
                new SampleEvent { EventId = 1 },
                new SampleEvent { EventId = 2 }
            };

            var eventApplicator = new Mock<IEventApplicator<DomainObject>>();

            eventApplicator
                .Setup(x => x.ApplyEvent(It.IsAny<DomainObject>(), It.IsAny<IPersistedDomainEvent>()))
                .Returns<DomainObject, IPersistedDomainEvent>((o, e) => o );

            var hydrator = new AggregateHydrator<DomainObject>(eventApplicator.Object);

            var domainObject = hydrator.Hydrate(new AggregateStream(Guid.Empty, 0, events, null));

            foreach (var @event in events) {
                eventApplicator.Verify(x => x.ApplyEvent(domainObject, @event));
            }
        }
    }
}
