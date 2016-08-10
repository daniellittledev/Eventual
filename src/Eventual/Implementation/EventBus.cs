using System;
using System.Threading.Tasks;
using Eventual.MessageContracts;

namespace Eventual.Implementation
{
    public class EventBus : IEventBus
    {
        private readonly Func<IDomainEvent, Task> eventHandler;

        public EventBus(Func<IDomainEvent, Task> eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        public Task PublishAsync(IDomainEvent domainEvent)
        {
            return eventHandler(domainEvent);
        }
    }
}
