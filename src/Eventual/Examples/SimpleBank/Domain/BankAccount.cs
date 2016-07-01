using System;

namespace Eventual.Examples.SimpleBank.Domain
{
    /// <summary>
    /// Immutable class!
    /// </summary>
    public class BankAccount : IAggregateRoot
    {
        internal decimal Balance { get; }

        protected BankAccount(Guid aggregateId, int loadedSequence)
        {
            AggregateId = aggregateId;
            LoadedSequence = loadedSequence;
        }

        public BankAccount(BankAccount previous, decimal balance)
            : this(previous.AggregateId, previous.LoadedSequence)
        {
            Balance = balance;
        }

        public Guid AggregateId { get; }
        public int LoadedSequence { get; }
    }
}
