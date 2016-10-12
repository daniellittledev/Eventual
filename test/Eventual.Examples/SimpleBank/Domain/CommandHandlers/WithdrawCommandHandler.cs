using System;
using System.Threading.Tasks;
using Enexure.MicroBus;
using Eventual.Examples.SimpleBank.Domain.Commands;

namespace Eventual.Examples.SimpleBank.Domain.CommandHandlers
{
    public class WithdrawCommandHandler : ICommandHandler<WithdrawCommand>
    {
        private IRepository<Account> repository;

        public WithdrawCommandHandler(IRepository<Account> repository)
        {
            this.repository = repository;
        }

        public async Task Handle(WithdrawCommand command)
        {
            var account = await repository.LoadAsync(command.AccountId);

            var events = account.Withdraw(command.Amount);

            repository.SaveAsync(account, events);
        }
    }
}
