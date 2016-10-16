using System;
using System.Threading.Tasks;
using Eventual.MessageContracts;

namespace Eventual.Implementation
{
    public class EventBus : IEventBus
    {
        private readonly Func<object, Task> eventHandler;

        public EventBus(Func<object, Task> eventHandler)
        {
            this.eventHandler = eventHandler;
        }

        public Task PublishAsync(object domainEvent)
        {
            return eventHandler(domainEvent);
        }
    }
}
