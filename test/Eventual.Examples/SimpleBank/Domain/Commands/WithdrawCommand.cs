using Enexure.MicroBus;
using System;

namespace Eventual.Examples.SimpleBank.Domain.Commands
{
    public class WithdrawCommand : ICommand
    {
        public Guid AccountId { get; }
        public decimal Amount { get; }

        public WithdrawCommand(Guid accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}