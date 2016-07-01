using Eventual.Examples.SimpleBank.Events;
using Eventual.MessageContracts;

namespace Eventual.Examples.SimpleBank.Domain
{
    /// <summary>
    /// Immutable class!
    /// </summary>
    public static class BankAccountDepositBehavior
    {
        public static IDomainEvent Deposit(this BankAccount bankAccount, decimal amount)
        {
            if (amount <= 0)
            {
                return BankAccountDepositFailureEvent.For("Amount must be greater than zero dollars");
            }

            return new BankAccountDepositEvent(amount);
        }

        public static BankAccount Apply(this BankAccount bankAccount, BankAccountDepositEvent fact)
        {
            return new BankAccount(bankAccount, bankAccount.Balance + fact.Amount);
        }
    }
}
