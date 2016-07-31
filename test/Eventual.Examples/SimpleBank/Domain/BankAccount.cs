using System;
using Eventual.Domain;

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
            Id = aggregateId;
            LoadedSequence = loadedSequence;
        }

        public BankAccount(BankAccount previous, decimal balance)
            : this(previous.Id, previous.LoadedSequence)
        {
            Balance = balance;
        }

        public Guid Id { get; }
        public int LoadedSequence { get; }
    }
}
