using Eventual.Domain;
using Eventual.EventStore;

namespace Eventual
{
    public interface IAggregateHydrator<T>
        where T : class, IAggregateRoot
    {
        T Hydrate(AggregateStream stream);
    }
}