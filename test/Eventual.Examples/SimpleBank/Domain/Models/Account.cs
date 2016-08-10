using System;
using Eventual.Domain;

namespace Eventual.Examples.SimpleBank.Domain
{
    /// <summary>
    /// Immutable class!
    /// </summary>
    public class Account : IAggregateRoot
    {
        internal decimal Balance { get; }

        protected Account(Guid id, int loadedSequence)
        {
            Id = id;
            LoadedSequence = loadedSequence;
        }

        public Account(Guid id)
            : this(id, 0)
        {
        }

        public Account(Account previous, decimal balance)
            : this(previous.Id, previous.LoadedSequence)
        {
            Balance = balance;
        }

        public Guid Id { get; }
        public int LoadedSequence { get; }
    }
}
