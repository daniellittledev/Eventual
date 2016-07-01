using System.Collections.Generic;

namespace Eventual
{
    public interface IAggregateHydrator
    {
        T Hydrate<T>(IEnumerable<object> events)
            where T : IAggregateRoot;
    }
}