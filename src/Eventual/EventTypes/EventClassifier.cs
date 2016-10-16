using System;
using System.Collections.Generic;
using System.Linq;
using Eventual.TypeDiscovery;

namespace Eventual.EventTypes
{
    public class EventClassifier : IEventClassifier
    {
        private readonly HashSet<Type> transientEvents;
        private readonly Dictionary<string, Type> alaises;

        public EventClassifier(IEventTypeContainer eventTypeContainer, IEnumerable<Type> transientEvents, IDictionary<Type, IReadOnlyCollection<string>> alaises)
        {
            this.transientEvents = new HashSet<Type>(transientEvents);
            this.alaises = alaises
                .SelectMany(x => x.Value.Select(alias => new {Alias = alias, EventType = x.Key}))
                .Concat(eventTypeContainer.EventTypes.Select(x => new { Alias = x.FullName, EventType = x }))
                .Distinct()
                .ToDictionary(x => x.Alias, x => x.EventType);
        }

        public bool IsPersistedEvent(Type eventType)
        {
            return !transientEvents.Contains(eventType);
        }

        public Type GetTypeForAliases(string eventAlias)
        {
            Type eventType;
            if (alaises.TryGetValue(eventAlias, out eventType))
            {
                return eventType;
            }

            throw new EventTypeNotFoundException(eventAlias);
        }
    }
}
