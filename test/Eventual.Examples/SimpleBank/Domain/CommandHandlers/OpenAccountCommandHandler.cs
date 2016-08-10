using System;
using System.Threading.Tasks;
using Enexure.MicroBus;
using Eventual.Examples.SimpleBank.Domain.Commands;

namespace Eventual.Examples.SimpleBank.Domain.CommandHandlers
{
    public class OpenAccountCommandHandler : ICommandHandler<OpenAccountCommand>
    {
        private IRepository<Account> repository;

        public OpenAccountCommandHandler(IRepository<Account> repository)
        {
            this.repository = repository;
        }

        public async Task Handle(OpenAccountCommand command)
        {
            var account = new Account(command.AccountId);

            var events = account.Open();

            await repository.SaveAsync(account, events);
        }
    }
}
