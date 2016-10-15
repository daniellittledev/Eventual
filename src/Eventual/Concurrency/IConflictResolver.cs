using System;
using System.Collections.Generic;

namespace Eventual.Concurrency
{
    public interface IConflictResolver
    {
        bool ConflictsWith(Type eventToCheck, IEnumerable<Type> previousEvents);
    }
}