using System;
using Eventual.MessageContracts;

namespace Eventual.Examples.SimpleBank.Events
{
    public class AccountDepositEvent : IPersistedDomainEvent
    {
        public decimal Amount { get; }

        public AccountDepositEvent(decimal amount)
        {
            Amount = amount;
        }
    }
}
