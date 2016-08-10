using Eventual.Examples.SimpleBank.Events;
using Eventual.MessageContracts;

namespace Eventual.Examples.SimpleBank.Domain
{
    /// <summary>
    /// Immutable class!
    /// </summary>
    public static class AccountWithdrawBehavior
    {
        public static IDomainEvent Withdraw(this Account Account, decimal amount)
        {
            var failures = new ValidationFailureList();

            if (amount <= 0)
            {
                failures.Add(nameof(amount), "Amount must be greater than zero dollars");
            }

            if (Account.Balance >= amount) {
                failures.Add(nameof(amount), "You cannot withdraw an amount greater than your current balance");
            }

            if (failures.HasFailures) {
                return failures.AsEvent();
            }

            return new AccountWithdrawnEvent(amount);
        }

        public static Account Apply(this Account account, AccountWithdrawnEvent @event)
        {
            return new Account(account, account.Balance - @event.Amount);
        }
    }
}
