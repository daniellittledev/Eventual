using Eventual.Examples.SimpleBank.Events;
using Eventual.MessageContracts;

namespace Eventual.Examples.SimpleBank.Domain
{
    /// <summary>
    /// Immutable class!
    /// </summary>
    public static class AccountDepositBehavior
    {
        public static IDomainEvent Deposit(this Account Account, decimal amount)
        {
            if (amount <= 0)
            {
                return new ValidationFailureList()
                    .Add(nameof(amount), "Amount must be greater than zero dollars")
                    .AsEvent();
            }

            return new AccountDepositEvent(amount);
        }

        public static Account Apply(this Account account, AccountDepositEvent @event)
        {
            return new Account(account, account.Balance + @event.Amount);
        }
    }
}
