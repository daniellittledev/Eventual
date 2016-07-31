using System;
using Eventual.Domain;
using Eventual.EventStore;

namespace Eventual.Implementation
{
    public class AggregateHydrator<T> : IAggregateHydrator<T>
        where T : class, IAggregateRoot
    {
        private readonly IEventApplicator<T> eventApplicator;

        public AggregateHydrator(IEventApplicator<T> eventApplicator)
        {
            this.eventApplicator = eventApplicator;
        }

        public T Hydrate(AggregateStream stream)
        {
            var aggregate = (T)Activator.CreateInstance(typeof(T), stream.AggregateId, stream.LatestSequence);

            foreach (var @event in stream.Events) {
                aggregate = eventApplicator.ApplyEvent(aggregate, @event);
            }

            return aggregate;
        }
    }
}