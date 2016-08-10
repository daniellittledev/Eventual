using Enexure.MicroBus;
using System;

namespace Eventual.Examples.SimpleBank.Domain.Commands
{
    public class OpenAccountCommand : ICommand
    {
        public Guid AccountId { get; }

        public OpenAccountCommand(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
