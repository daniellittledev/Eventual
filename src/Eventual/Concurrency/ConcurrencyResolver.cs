﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventual.Concurrency
{
    public class ConflictResolver : IConflictResolver
    {
        private readonly Dictionary<Type, List<Type>> eventsThatDontConflict = new Dictionary<Type, List<Type>>();

        public bool ConflictsWith(Type eventToCheck, IEnumerable<Type> events)
        {
            List<Type> safeEvents;
            return !eventsThatDontConflict.TryGetValue(eventToCheck, out safeEvents) 
                || events.Any(newEvent => !safeEvents.Contains(newEvent));
        }

        public ConflictResolver RegisterNoConflict(Type eventDefinition, params Type[] doesNotConflictsWith)
        {
            return RegisterNoConflict(eventDefinition, (IEnumerable<Type>) doesNotConflictsWith);
        }

        public ConflictResolver RegisterNoConflict(Type eventDefinition, IEnumerable<Type> doesNotConflictsWith)
        {
            List<Type> safeEvents;
            if (!eventsThatDontConflict.TryGetValue(eventDefinition, out safeEvents)) {
                eventsThatDontConflict.Add(eventDefinition, (safeEvents = new List<Type>()));
            }

            safeEvents.AddRange(doesNotConflictsWith);
            return this;
        }
    }
}
