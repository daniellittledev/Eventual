using System;
using Eventual.MessageContracts;

namespace Eventual.Examples.SimpleBank.Events
{
    public class AccountOpenedEvent : IDomainEvent
    {
        public AccountOpenedEvent()
        {
        }
    }
}
