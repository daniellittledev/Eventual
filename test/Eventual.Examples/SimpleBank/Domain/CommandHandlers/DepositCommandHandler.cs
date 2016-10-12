using System.Threading.Tasks;
using Enexure.MicroBus;
using Eventual.Examples.SimpleBank.Domain.Commands;

namespace Eventual.Examples.SimpleBank.Domain.CommandHandlers
{
    public class DepositCommandHandler : ICommandHandler<WithdrawCommand>
    {
        private IRepository<Account> repository;

        public DepositCommandHandler(IRepository<Account> repository)
        {
            this.repository = repository;
        }

        public async Task Handle(WithdrawCommand command)
        {
            var account = await repository.LoadAsync(command.AccountId);

            var events = account.Deposit(command.Amount);

            await repository.SaveAsync(account, events);
        }
    }
}
