using System;
using System.Collections.Generic;
using System.Reflection;

namespace Eventual.TypeDiscovery
{
    public class DomainEventLocator : IScanningLocator, IEventTypeContainer
    {
        private readonly Func<Type, TypeInfo, bool> eventPredicate;
        private readonly List<Type> domainEvents = new List<Type>();

        public IReadOnlyCollection<Type> EventTypes => domainEvents;

        public DomainEventLocator(Func<Type, TypeInfo, bool> eventPredicate)
        {
            this.eventPredicate = eventPredicate;
        }

        public void Scan(Type type, TypeInfo typeInfo)
        {
            if (eventPredicate(type, typeInfo)) {
                domainEvents.Add(type);
            }
        }
    }
}
