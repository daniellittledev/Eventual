using Eventual.Domain;
using Eventual.MessageContracts;

namespace Eventual
{
    public interface IEventApplicator
    {
        T ApplyEvent<T>(T aggregate, IPersistedDomainEvent @event)
            where T : class, IAggregateRoot;
    }
}