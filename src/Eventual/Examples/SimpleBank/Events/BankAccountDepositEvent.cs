using System;
using Eventual.MessageContracts;

namespace Eventual.Examples.SimpleBank.Events
{
    public class BankAccountDepositEvent : IPersistedDomainEvent
    {
        public decimal Amount { get; }

        public BankAccountDepositEvent(decimal amount)
        {
            Amount = amount;
        }
    }
}
