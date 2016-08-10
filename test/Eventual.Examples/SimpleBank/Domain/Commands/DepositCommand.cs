using Enexure.MicroBus;
using System;

namespace Eventual.Examples.SimpleBank.Domain.Commands
{
    public class DepositCommand : ICommand
    {
        public Guid AccountId { get; }
        public decimal Amount { get; }

        public DepositCommand(Guid accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}