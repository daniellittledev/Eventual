using System;
using Eventual.MessageContracts;

namespace Eventual.Examples.SimpleBank.Events
{
    public class AccountWithdrawnEvent : IPersistedDomainEvent
    {
        public decimal Amount { get; }

        public AccountWithdrawnEvent(decimal amount)
        {
            Amount = amount;
        }
    }
}
