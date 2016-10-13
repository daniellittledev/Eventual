using System;
using System.Collections.Generic;
using Eventual.MessageContracts;
using Eventual.Domain;
using System.Reflection;

namespace Eventual.Implementation
{
    public class EventApplicator : IEventApplicator
    {
        private readonly Dictionary<Type, IApplyMethod> eventApplyMethods = new Dictionary<Type, IApplyMethod>();

        public EventApplicator(IReadOnlyCollection<IApplyMethod> eventApplyMethods)
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

                if (!methodInfo.AggregateRootType.IsAssignableFrom(typeof(T))) {

                    throw new EventApplyAggregateMismatchException(typeof(T), methodInfo.AggregateRootType, methodInfo.EventType);
                }

                return (T)methodInfo.Invoke(aggregate, @event);
            } else {
                throw new ApplyMethodNotFoundException(eventType);
            }
        }
    }
}
