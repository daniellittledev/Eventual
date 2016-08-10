using System;
using Eventual.MessageContracts;

namespace Eventual.Examples.SimpleBank.Events
{
    public class AccountOpenedEvent : IPersistedDomainEvent
    {
        public AccountOpenedEvent()
        {
        }
    }
}
