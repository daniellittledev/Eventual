using Eventual.Domain;
using Eventual.MessageContracts;

namespace Eventual
{
    public interface IEventApplicator
    {
        T ApplyEvent<T>(T aggregate, IPersistedDomainEvent @event)
            where T : class, IAggregateRoot;
    }

    public interface IEventApplicator<T>
        where T : class, IAggregateRoot
    {
        T ApplyEvent(T aggregate, IPersistedDomainEvent @event);
    }
}