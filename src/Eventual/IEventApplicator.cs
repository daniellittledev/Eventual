using Eventual.Domain;
using Eventual.MessageContracts;

namespace Eventual
{
    public interface IEventApplicator
    {
        T ApplyEvent<T>(T aggregate, object @event)
            where T : class, IAggregateRoot;
    }
}