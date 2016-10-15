using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventual.Concurrency
{
    public class ConflictResolver : IConflictResolver
    {
        private readonly Dictionary<Type, List<Type>> _conflictSafeRegister;

        public ConflictResolver()
        {
            _conflictSafeRegister = new Dictionary<Type, List<Type>>();
        }

        public bool ConflictsWith(Type eventToCheck, IEnumerable<Type> previousEvents)
        {
            List<Type> safeEvents;
            return !_conflictSafeRegister.TryGetValue(eventToCheck, out safeEvents)
                || previousEvents.All(x => safeEvents.Contains(x));
        }

        public void RegisterNoConflict(Type eventDefinition, IEnumerable<Type> doesNotConflictsWith)
        {
            List<Type> safeEvents;
            if (!_conflictSafeRegister.TryGetValue(eventDefinition, out safeEvents)) {
                safeEvents = new List<Type>();
                _conflictSafeRegister.Add(eventDefinition, safeEvents);
            }

            safeEvents.AddRange(doesNotConflictsWith);
        }
    }
}
