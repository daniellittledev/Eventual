using System;
using System.Collections.Generic;
using Eventual.MessageContracts;

namespace Eventual.Examples.SimpleBank.Events
{
    public class BankAccountDepositFailureEvent : IDomainEvent
    {
        public IReadOnlyCollection<string> Reasons { get; }

        public BankAccountDepositFailureEvent(IReadOnlyCollection<string> reasons)
        {
            this.Reasons = reasons;
        }

        public static BankAccountDepositFailureEvent For(string reason)
        {
            return new BankAccountDepositFailureEvent(new [] { reason });
        }
    }
}
