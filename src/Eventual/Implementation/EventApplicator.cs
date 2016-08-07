using System;
using System.Collections.Generic;
using Eventual.MessageContracts;
using Eventual.Domain;
using System.Reflection;

namespace Eventual.Implementation
{
    public class EventApplicator<T> : IEventApplicator<T>
        where T : class, IAggregateRoot
    {
        private readonly IEventApplicator eventApplicator;

        public EventApplicator(IEventApplicator eventApplicator)
        {
            this.eventApplicator = eventApplicator;
        }

        public T ApplyEvent(T aggregate, IPersistedDomainEvent @event)
        {
            return eventApplicator.ApplyEvent(aggregate, @event);
        }
    }

    public class EventApplicator : IEventApplicator
    {
        private readonly Dictionary<Type, IApplyMethod> eventApplyMethods = new Dictionary<Type, IApplyMethod>();

        public EventApplicator(EventApplyMethods eventApplyMethods)
        {
            foreach (var eventApplyMethod in eventApplyMethods) {
                this.eventApplyMethods.Add(eventApplyMethod.EventType, eventApplyMethod);
            }
        }

        public T ApplyEvent<T>(T aggregate, IPersistedDomainEvent @event)
            where T : class, IAggregateRoot
        {
            var eventType = @event.GetType();

            IApplyMethod methodInfo;
            if (eventApplyMethods.TryGetValue(eventType, out methodInfo)) {
                return (T)methodInfo.Invoke(aggregate, @event);
            } else {
                throw new ApplyMethodNotFoundException(eventType);
            }
        }
    }
}
