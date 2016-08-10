using Eventual.Examples.SimpleBank.Events;
using Eventual.MessageContracts;

namespace Eventual.Examples.SimpleBank.Domain
{
    /// <summary>
    /// Immutable class!
    /// </summary>
    public static class AccountOpenBehaviour
    {
        public static IDomainEvent Open(this Account Account)
        {
            return new AccountOpenedEvent();
        }

        public static Account Apply(this Account account, AccountOpenedEvent @event)
        {
            return account;
        }
    }
}
